using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Project
{

    public class UpdateProjectDto
    {
        public int id { get; set; }

        public string name { get; set; }

        public METHOD? primaryMethod { get; set; }

        public STATUS? projectStatus { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

    }
}