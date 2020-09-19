using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Project : IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("primaryMethod")]
        public METHOD? PrimaryMethod { get; set; }

        [BsonElement("projectStatus")]
        public STATUS? ProjectStatus { get; set; }

        [BsonElement("dominantStat")]
        public STATS? DominantStat { get; set; }

        [BsonElement("stats")]
        public STATS[] Stats { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("projectUsages")]
        public string[] ProjectUsages { get; set; }

        [BsonElement("totalTimeSpend")]
        public int TotalTimeSpend { get; set; }

        [BsonElement("dateAdded")]
        public DateTime DateAdded { get; set; }
        public DateTime? timeCreated { get; set; }

        public DateTime? timeUpdated { get; set; }
    }
}