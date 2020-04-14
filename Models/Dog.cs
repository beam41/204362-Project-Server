using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MheanMaa.Models
{
    public class Dog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string[] Name { get; set; }

        public string Breed { get; set; }

        public int AgeYear { get; set; }

        public int AgeMonth { get; set; }

        public string Sex { get; set; }

        public string Description { get; set; }

        public bool IsAlive { get; set; }

        public string CollarColor { get; set; }

        public string Caretaker { get; set; }

        public string[] CaretakerPhone { get; set; }

        public string Location { get; set; }

        public string ImgPath { get; set; }

        public int DeptNo { get; set; }
    }
    public class DogList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string[] Name { get; set; }

        public int AgeYear { get; set; }

        public int AgeMonth { get; set; }

        public string Sex { get; set; }

        public string Description { get; set; }

        public bool IsAlive { get; set; }

        public string CollarColor { get; set; }

        public string Caretaker { get; set; }
    }

    public class DogVisitor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }

        public string[] Name { get; set; }

        public string ImgPath { get; set; }
    }
}
