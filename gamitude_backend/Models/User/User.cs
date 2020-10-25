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

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

    }
}