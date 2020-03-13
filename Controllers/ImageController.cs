﻿using System;
using System.IO;
using System.Threading.Tasks;
using MheanMaa.Models;
using MheanMaa.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImageUpload.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {

        public IWebHostEnvironment _environment;
        public IImageSettings _settings;

        public ImageController(IWebHostEnvironment environment, IImageSettings settings)
        {
            _environment = environment;
            _settings = settings;
        }

        [HttpPost]
        public ActionResult Post([FromForm]ImageData img)
        {
            // Getting Image
            IFormFile imageDat = img.Data;
            // load image
            using (MemoryStream memoryStream = new MemoryStream())
            {
                imageDat.CopyTo(memoryStream);
                //convertion
                using (Image<Rgba32> image = Image.Load(memoryStream.ToArray()))
                {
                    double ratio = Convert.ToDouble(image.Width) / image.Height;

                    if (image.Width > image.Height && image.Width > _settings.MaxWidth)
                    {
                        image.Mutate(x => x
                         .Resize(_settings.MaxWidth, (int) Math.Round(_settings.MaxWidth / ratio))
                        );
                    }

                    if (image.Width < image.Height && image.Height > _settings.MaxWidth)
                    {
                        image.Mutate(x => x
                         .Resize((int) Math.Round(_settings.MaxWidth * ratio), _settings.MaxWidth)
                        );
                    }

                    string guid = Guid.NewGuid().ToString();
                    string newFName = guid + ".jpg";
                    try
                    {
                        if (!Directory.Exists(Path.Join(_environment.WebRootPath, "uploads")))
                            Directory.CreateDirectory(Path.Join(_environment.WebRootPath, "uploads"));
                        JpegEncoder encoder = new JpegEncoder()
                        {
                            Quality = 60
                        };
                        image.Save(Path.Join(_environment.WebRootPath, "uploads", newFName), encoder);
                        return Accepted(new { fileName = newFName });
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}