using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class UserTimers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("timers")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String[] timers { get; set; } = new String[]{};

    }
}