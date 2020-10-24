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
        public String id { get; set; }

        [BsonElement("userId")]
        public String userId { get; set; }

        [BsonElement("name")]
        public String name { get; set; }

        [BsonElement("description")]
        public String description { get; set; }

        [BsonElement("icon")]
        public String icon { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
    }
}