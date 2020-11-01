using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class CreateTimerDto
    {
        [Required]
        public int? workTime { get; set; }

        [Required]
        public int? breakTime { get; set; }

        [Required]
        public int? overTime { get; set; }
        
        public string name { get; set; }

        public int? longerBreakTime { get; set; }

        public int? breakInterval { get; set; }

    }
}