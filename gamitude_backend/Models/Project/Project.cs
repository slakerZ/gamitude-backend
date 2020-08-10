using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gamitude_backend.Models
{
    public class Project : IBaseEntity
    {
        public int id { get; set; }

        [MaxLength(255)]
        public string userId { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }

        [MaxLength(255)]
        public string name { get; set; }

        public METHOD? primaryMethod { get; set; }

        public STATUS? projectStatus { get; set; }

        public STATS? dominantStat { get; set; }

        public STATS[] stats { get; set; }

        public int totalTimeSpend { get; set; }

        public DateTime? timeCreated { get; set; }

        public DateTime? timeUpdated { get; set; }
    }
}