using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class TimeSpend
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("projectId")]
        public string ProjectId { get; set; }

        [BsonElement("projectType")]
        public PROJECT_TYPE? ProjectType { get; set; }

        [BsonElement("timeSpend")]
        public int Duration { get; set; }

        [BsonElement("dominantStat")]
        public STATS? DominantStat { get; set; }

        [BsonElement("stats")]
        public STATS[] Stats { get; set; }

        /// <summary>
        /// Calculates the wages. Values depends on how many stats u wanna boost.
        /// </summary>
        public Dictionary<STATS, int> getWages()
        {
            Dictionary<STATS, int> wageMap = new Dictionary<STATS, int>();
            int dominantStat = 0;
            int stat = 0;
            switch (Stats.GetLength(0))
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
            foreach (var s in Stats)
            {
                if (s == DominantStat)
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