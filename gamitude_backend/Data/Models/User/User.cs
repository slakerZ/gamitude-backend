using System;
using System.Collections.Generic;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class User : MongoUser
    {

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
        
        [BsonElement("currentRankId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string currentRankId { get; set; }

        [BsonElement("purchasedRankIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> purchasedRankIds { get; set; } = new List<string>();

        [BsonElement("currentThemeId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string currentThemeId { get; set; }

        [BsonElement("purchasedThemeIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> purchasedThemeIds { get; set; } = new List<string>();

        [BsonElement("timers")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> timers { get; set; } = new List<string>();
    }
}