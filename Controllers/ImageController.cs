using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MheanMaa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public ActionResult Post([FromForm]ImageFormData img)
        {
            // Getting Image
            IFormFile imageDat = img.Data;
            if (img.Data.ContentType == "image/jpeg" || img.Data.ContentType == "image/png")
            {
                // load image
                try
                {
                    string fileName = _imageService.SaveImg(imageDat);
                    return Accepted(new { fileName });
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return BadRequest();
        }
    }
}