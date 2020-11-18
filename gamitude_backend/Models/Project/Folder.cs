using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Folder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("description")]
        public string description { get; set; }

        [BsonElement("icon")]
        public string icon { get; set; } = "";

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
    }
}