using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class CreateTimerDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "At least 1")]
        public int? workTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "At least 1")]
        public int? breakTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "At least 1")]
        public int? overTime { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "At least 2 characters required ")]  
        public string name { get; set; }

        [Required]
        [MaxLength(2, ErrorMessage = "Cannot excede 2 characters ")]  
        public string label { get; set; }

        [Required]
        public TIMER_TYPE? timerType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "At least 1")]
        public int? longerBreakTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "At least 1")]
        public int? breakInterval { get; set; }

    }
}