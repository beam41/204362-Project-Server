using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;


namespace MheanMaa.Models
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Accepted { get; set; }

        public double AcceptedDate { get; set; }

        public string Reporter { get; set; }

        public string Accepter { get; set; }

        public string ImgPath { get; set; }
        
        public int DeptNo { get; set; }
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

    public class ReportVisitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public double AcceptedDate { get; set; }

        public string Description { get; set; }

        public string ImgPath { get; set; }

        public static explicit operator ReportVisitor(Report r)
        {
            return new ReportVisitor
            {
                Id = r.Id,
                Title = r.Title,
                AcceptedDate = r.AcceptedDate,
                Description = r.Description,
                ImgPath = r.ImgPath,
            };
        }
    }
}