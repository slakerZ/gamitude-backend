using gamitude_backend.Models;

namespace gamitude_backend.Dto.Timer
{

    public class UpdateTimerDto
    {
        public string name { get; set; }

        public string label { get; set; }

        public int? workTime { get; set; }

        public int? breakTime { get; set; }

        public int? overTime { get; set; }

        public TIMER_TYPE? timerType { get; set; }
        
        public int? longerBreakTime { get; set; }

        public int? breakInterval { get; set; }
    }
}