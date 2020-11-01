using System;
using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class UpdateTimerDto
    {
        public string name { get; set; }

        public int? workTime { get; set; }

        public int? breakTime { get; set; }

        public int? overTime { get; set; }
        
        public int? longerBreakTime { get; set; }

        public int? breakInterval { get; set; }
    }
}