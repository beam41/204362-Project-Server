using System;
using MheanMaa.Models;
using MheanMaa.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageUpload.Controllers
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
        public ActionResult Post([FromForm]ImageData img)
        {
            // Getting Image
            IFormFile imageDat = img.Data;
            // load image
            try
            {
                string fileName = _imageService.SaveImg(imageDat, true);
                return Accepted(new { fileName });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}