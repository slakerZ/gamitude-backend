using System.ComponentModel.DataAnnotations;
using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class UpdateTimerDto
    {
        [MinLength(2, ErrorMessage = "{0} at least 2 characters required")]
        public string name { get; set; }

        [MaxLength(2, ErrorMessage = "{0} cannot excede 2 characters ")]
        [MinLength(1, ErrorMessage = "{0} least 1 characters required")]
        public string label { get; set; }

        public TIMER_TYPE? timerType { get; set; }
        
        public UpdateCountDownInfoDto countDownInfo { get; set; }
    }

    public class UpdateCountDownInfoDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? workTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? breakTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? overTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? longerBreakTime { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} at least 1 required")]
        public int? breakInterval { get; set; }
    }
}