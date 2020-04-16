using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MheanMaa.Models
{
    public class Donate
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public bool Accepted { get; set; }

        public long AcceptedOn { get; set; }

        public string Description { get; set; }

        public string QrLink { get; set; }

        public string ImgPath { get; set; }

        public int DeptNo { get; set; }
    }

    public class DonateList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Creator { get; set; }

        public bool Accepted { get; set; }
    }

    public class DonateVisitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public double AcceptedOn { get; set; }

        public string Description { get; set; }

        public string QrLink { get; set; }

        public string ImgPath { get; set; }

        public static explicit operator DonateVisitor(Donate d)
        {
            return new DonateVisitor
            {
                Id = d.Id,
                Title = d.Title,
                AcceptedOn = d.AcceptedOn,
                Description = d.Description,
                QrLink = d.QrLink,
                ImgPath = d.ImgPath,
            };
        }
    }
}
