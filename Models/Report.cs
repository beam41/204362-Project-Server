using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace MheanMaa.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Reporter { get; set; }

        public string ReporterContact { get; set; }

        public string ImgPath { get; set; }

        public bool Accepted { get; set; }

        public long AcceptedOn { get; set; }

        public string AcceptedBy { get; set; }
    }

    public class ReportList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Reporter { get; set; }

        public bool Accepted { get; set; }
    }
}