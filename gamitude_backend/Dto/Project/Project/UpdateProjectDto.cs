using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Project
{

    public class UpdateProjectDto
    {
        public string name { get; set; }

        public String folderId { get; set; }

        public String defaultTimerId { get; set; }

        public PROJECT_TYPE? projectType { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public int? daysPerWeek { get; set; }

        public int? hoursPerDay { get; set; }

        public int? dayInterval { get; set; }
    }
}