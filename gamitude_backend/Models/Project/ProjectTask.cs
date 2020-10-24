using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class ProjectTask
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("projectId")]
        public string projectId { get; set; }

        [BsonElement("description")]
        public string description { get; set; }

        [BsonElement("time")]
        public int time { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

    }
}