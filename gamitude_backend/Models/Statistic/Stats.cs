using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;


using gamitude_backend.Settings;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using gamitude_backend.Configuration;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Stats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("strength")]
        public long strength { get; set; }

        [BsonElement("intelligence")]
        public long intelligence { get; set; }

        [BsonElement("fluency")]
        public long fluency { get; set; }

        [BsonElement("creativity")]
        public long creativity { get; set; }


    }

}