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

        public string Detail { get; set; }

        public string Writer { get; set; }

        public long WroteOn { get; set; }

        public bool Accepted { get; set; }

        public string AcceptedBy { get; set; }

        public long AcceptedOn { get; set; }

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

        public string Writer { get; set; }

        public long AcceptedOn { get; set; }

        public string Detail { get; set; }

        public string ImgPath { get; set; }

        public static explicit operator NewsVisitor(News n)
        {
            return new NewsVisitor
            {
                Id = n.Id,
                Title = n.Title,
                Writer = n.Writer,
                AcceptedOn = n.AcceptedOn,
                Detail = n.Detail.Substring(0, 128 % n.Detail.Length),
                ImgPath = n.ImgPath,
            };
        }
    }
}