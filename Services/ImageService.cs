using MheanMaa.Settings;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace MheanMaa.Services
{
    public class ImageService
    {
        public IWebHostEnvironment _environment;
        public IImageSettings _settings;

        public ImageService(IWebHostEnvironment environment, IImageSettings settings)
        {
            _environment = environment;
            _settings = settings;
        }

        public string SaveImg(IFormFile imageDat, bool genPlacehold)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                imageDat.CopyTo(memoryStream);
                //convertion
                using (Image<Rgba32> image = Image.Load(memoryStream.ToArray()))
                {
                    image.Metadata.ExifProfile = null;

                    double ratio = Convert.ToDouble(image.Width) / image.Height;

                    if (image.Width >= image.Height && image.Width > _settings.MaxWidth)
                    {
                        image.Mutate(x => x
                         .Resize(_settings.MaxWidth, (int)Math.Round(_settings.MaxWidth / ratio))
                        );
                    }
                    else if (image.Width < image.Height && image.Height > _settings.MaxWidth)
                    {
                        image.Mutate(x => x
                         .Resize((int)Math.Round(_settings.MaxWidth * ratio), _settings.MaxWidth)
                        );
                    }

                    string guid = Guid.NewGuid().ToString();
                    string newFName = guid + ".jpg";
                    try
                    {
                        if (!Directory.Exists(Path.Join(_environment.WebRootPath, "uploads")))
                            Directory.CreateDirectory(Path.Join(_environment.WebRootPath, "uploads"));
                        if (!Directory.Exists(Path.Join(_environment.WebRootPath, "placeholder")))
                            Directory.CreateDirectory(Path.Join(_environment.WebRootPath, "placeholder"));
                        JpegEncoder encoder = new JpegEncoder()
                        {
                            Quality = 60
                        };
                        image.Save(Path.Join(_environment.WebRootPath, "uploads", newFName), encoder);

                        // really smol image
                        image.Mutate(x => x
                         .Resize((int)Math.Round(image.Width / 10.0), (int)Math.Round(image.Height / 10.0))
                        );

                        if (genPlacehold)
                        {
                            JpegEncoder encoder2 = new JpegEncoder()
                            {
                                Quality = 10
                            };
                            image.Save(Path.Join(_environment.WebRootPath, "placeholder", newFName), encoder2);
                        }
                        
                        return newFName;
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
