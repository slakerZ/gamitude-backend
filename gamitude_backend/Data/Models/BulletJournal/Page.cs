using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Page
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

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("beetwenDays")]
        public BeetwenDays beetwenDays { get; set; }

        [BsonElement("icon")]
        public string icon { get; set; } = "";

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

        [BsonElement("description")]
        public string description { get; set; }

        [BsonElement("pageType")]
        public PAGE_TYPE pageType { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class BeetwenDays
    {
        [BsonElement("fromDay")]
        public int fromDay { get; set; }

        [BsonElement("toDay")]
        public int toDay { get; set; }
    }

}