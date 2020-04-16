using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MheanMaa.Models
{
    public class News
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Accepted { get; set; }

        public string Writer { get; set; }

        public double WriteDate { get; set; }

        public double AcceptedDate { get; set; }

        public string ImgPath { get; set; }

        public int DeptNo { get; set; }
    }
    public class NewsList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string Title { get; set; }

        public string Writer { get; set; }

        public bool Accepted { get; set; }
    }

    public class NewsVisitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }

        public double AcceptedDate { get; set; }

        public string Description { get; set; }

        public string ImgPath { get; set; }

        public static explicit operator NewsVisitor(News n)
        {
            return new NewsVisitor
            {
                Id = n.Id,
                Title = n.Title,
                AcceptedDate = n.AcceptedDate,
                Description = n.Description,
                ImgPath = n.ImgPath,
            };
        }
    }
}