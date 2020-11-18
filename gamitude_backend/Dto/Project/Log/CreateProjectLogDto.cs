using System;
using gamitude_backend.Models;
namespace gamitude_backend.Dto.Project
{
    public class CreateProjectLogDto
    {

        public string projectId { get; set; }

        public string projectTaskId { get; set; }

        public string log { get; set; }

        public int timeSpend { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public PROJECT_TYPE? projectType { get; set; }

    }
}