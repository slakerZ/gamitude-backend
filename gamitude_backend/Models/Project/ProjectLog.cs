using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class ProjectLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("projectId")]
        public string projectId { get; set; }

        [BsonElement("projectTaskId")]
        public string projectTaskId { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("log")]
        public string log { get; set; }

        [BsonElement("timeSpend")]
        public int timeSpend { get; set; }

        [BsonElement("dominantStat")]
        public STATS? dominantStat { get; set; }

        [BsonElement("stats")]
        public STATS[] stats { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }

    }
}