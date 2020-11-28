using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Project
{

    public class CreateProjectDto
    {

        [Required]
        public string name { get; set; }

        [Required]
        public string folderId { get; set; }

        public string defaultTimerId { get; set; }

        [Required]
        public PROJECT_TYPE? projectType { get; set; }

        [Required]
        public STATS? dominantStat { get; set; }

        [Required]
        public STATS[] stats { get; set; }

        public int? daysPerWeek { get; set; }

        public int? hoursPerDay { get; set; }

        public int? dayInterval { get; set; }

    }
}