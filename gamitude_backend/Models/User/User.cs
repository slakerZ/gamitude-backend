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
        public string name { get; set; }

        public Rank currentRank { get; set; }
        public Theme currentTheme { get; set; }

        public List<Rank> purchasedRanks { get; set; }
        public List<Theme> purchasedThemes { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

    }
}