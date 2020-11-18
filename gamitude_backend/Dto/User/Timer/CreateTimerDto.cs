using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class CreateTimerDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? workTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? breakTime { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? overTime { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "{0} at least 2 characters required")]  
        public string name { get; set; }

        [Required]
        [MaxLength(2, ErrorMessage = "{0} cannot excede 2 characters ")]  
        [MinLength(1, ErrorMessage = "{0} least 1 characters required")]  
        public string label { get; set; }

        [Required]
        public TIMER_TYPE? timerType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? longerBreakTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? breakInterval { get; set; }

    }
}