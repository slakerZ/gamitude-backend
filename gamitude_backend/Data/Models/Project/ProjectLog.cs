using System;
using System.Collections.Generic;
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
        [BsonRepresentation(BsonType.ObjectId)]
        public string projectId { get; set; }

        [BsonElement("projectTaskId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string projectTaskId { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("projectType")]
        public PROJECT_TYPE? projectType { get; set; }

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

        /// <summary>
        /// Calculates the wages. Values depends on how many stats u wanna boost.
        /// </summary>
        public Dictionary<STATS, int> getWages()
        {
            Dictionary<STATS, int> wageMap = new Dictionary<STATS, int>();
            int dominantStat = 0;
            int stat = 0;
            switch (stats.GetLength(0))
            {
                case 1:
                    stat = 0;
                    dominantStat = 1;
                    break;
                case 2:
                    stat = 3;
                    dominantStat = 4;
                    break;
                case 3:
                    stat = 2;
                    dominantStat = 3;
                    break;
                case 4:
                    stat = 2;
                    dominantStat = 4;
                    break;

            }
            foreach (var s in stats)
            {
                if (s == this.dominantStat)
                {
                    wageMap.Add(s, dominantStat);
                }
                else
                {
                    wageMap.Add(s, stat);
                }
            }
            return wageMap;
        }
    }
}