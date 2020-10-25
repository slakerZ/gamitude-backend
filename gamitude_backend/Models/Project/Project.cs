using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Project
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("folderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String folderId { get; set; }

        [BsonElement("defaultTimerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String defaultTimerId { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("projectType")]
        public PROJECT_TYPE projectType { get; set; }

        [BsonElement("dominantStat")]
        public STATS? dominantStat { get; set; }

        [BsonElement("stats")]
        public STATS[] stats { get; set; }

        [BsonElement("totalTimeSpend")]
        public int timeSpend { get; set; }

        [BsonElement("daysPerWeek")]
        public int? daysPerWeek { get; set; }

        [BsonElement("hoursPerDay")]
        public int? hoursPerDay { get; set; }

        [BsonElement("dayInterval")]
        public int? dayInterval { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
    }
}