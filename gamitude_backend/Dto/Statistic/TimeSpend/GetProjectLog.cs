using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    public class GetProjectLog
    {

        public string log { get; set; }

        public int timeSpend { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public DateTime dateCreated { get; set; }

    }
}