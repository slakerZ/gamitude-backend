using System;
using gamitude_backend.Models;
namespace gamitude_backend.Dto.Project
{
    public class GetProjectLogDto
    {
        public string id { get; set; }

        public string log { get; set; }

        public int timeSpend { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public DateTime dateCreated { get; set; }

    }
}