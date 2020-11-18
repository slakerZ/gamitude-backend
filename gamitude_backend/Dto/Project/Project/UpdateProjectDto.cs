using gamitude_backend.Models;

namespace gamitude_backend.Dto.Project
{

    public class UpdateProjectDto
    {
        public string name { get; set; }

        public string folderId { get; set; }

        public string defaultTimerId { get; set; }

        public PROJECT_TYPE? projectType { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public int? daysPerWeek { get; set; }

        public int? hoursPerDay { get; set; }

        public int? dayInterval { get; set; }
    }
}