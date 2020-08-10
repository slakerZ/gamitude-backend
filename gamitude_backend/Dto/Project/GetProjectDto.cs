using System;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Project
{

    public class GetProjectDto
    {
        public int id { get; set; }

        public string name { get; set; }

        public METHOD? primaryMethod { get; set; }

        public STATUS? projectStatus { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public int totalTimeSpend { get; set; }

        public DateTime? timeCreated { get; set; }

    }
}