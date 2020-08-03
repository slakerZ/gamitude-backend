using System;

namespace gamitude_backend.Models
{
    public class Project
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public METHOD? PrimaryMethod { get; set; }

        public STATUS? ProjectStatus { get; set; }

        public STATS? DominantStat { get; set; }

        public STATS[] Stats { get; set; }

        public string[] ProjectUsages { get; set; }

        public int TotalTimeSpend { get; set; }

        public DateTime DateAdded { get; set; }
    }
}