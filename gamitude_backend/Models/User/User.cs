using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class User : MongoUser
    {
        [PersonalData]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("timeCreated")]
        public DateTime? timeCreated { get; set; }

        [BsonElement("timeUpdated")]
        public DateTime? timeUpdated { get; set; }

        [BsonElement("rankSet")]
        public string RankSet { get; set; }

        [BsonElement("tier")]
        public string Tier { get; set; }

        [BsonElement("dateAdded")]
        public DateTime DateAdded { get; set; }

    }
}