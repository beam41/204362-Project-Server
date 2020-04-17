using Microsoft.AspNetCore.Http;

namespace MheanMaa.Models
{
    public class ReportFormData
    {
        public string Reporter { get; set; }

        public string ReporterContact { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public IFormFile Img { get; set; }
    }
}
