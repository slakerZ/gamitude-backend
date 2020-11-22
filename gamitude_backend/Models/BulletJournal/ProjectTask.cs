using System;
using System.Collections.Generic;
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

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("journalId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string journalId { get; set; }

        [BsonElement("projectId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string projectId { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("description")]
        public string description { get; set; }

        [BsonElement("note")]
        public string note { get; set; }

        [BsonElement("tags")]
        public List<string> tags { get; set; }

        [BsonElement("deadLine")]
        public DateTime deadLine { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

        [BsonElement("dateFinished")]
        public DateTime? dateFinished { get; set; }

    }
}