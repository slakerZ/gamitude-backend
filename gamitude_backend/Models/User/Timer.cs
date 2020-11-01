using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Timer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("workTime")]
        public int workTime { get; set; }

        [BsonElement("breakTime")]
        public int breakTime { get; set; }
        
        [BsonElement("overTime")]
        public int overTime { get; set; }

        [BsonElement("longerBreakTime")]
        public int? longerBreakTime { get; set; }

        [BsonElement("breakInterval")]
        public int? breakInterval { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
    }
}