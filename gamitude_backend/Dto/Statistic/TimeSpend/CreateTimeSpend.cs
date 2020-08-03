using gamitude_backend.Models;

namespace gamitude_backend.Dto.TimeSpend
{
    public class CreateTimeSpend
    {


        public string ProjectId { get; set; }

        public PROJECT_TYPE? ProjectType { get; set; }

        public int Duration { get; set; }
        
        public STATS? DominantStat { get; set; }

        public STATS[] Stats { get; set; }

    }
}
