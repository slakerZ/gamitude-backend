using Microsoft.Extensions.Options;
using gamitude_backend.Settings;
using gamitude_backend.Configuration;

namespace gamitude_backend.Dto.Stats
{
    public class GetLastWeekAvgStatsDto
    {

        public int Strength { get; set; }

        public int Intelligence { get; set; }

        public int Fluency { get; set; }

        public int Creativity { get; set; }

        public GetLastWeekAvgStatsDto weekAvg()
        {
            this.Strength /= 7;
            this.Intelligence /= 7;
            this.Fluency /= 7;
            this.Creativity /= 7;
            return this;
        }

        public GetLastWeekAvgStatsDto scaleToPercent()
        {
            this.Creativity =(this.Creativity*100)/ StaticValues.dayLenght;
            this.Fluency = (this.Fluency*100)/StaticValues.dayLenght;
            this.Intelligence =(this.Intelligence*100)/ StaticValues.dayLenght;
            this.Strength = (this.Strength *100)/StaticValues.dayLenght;
            return this;
        }
    }
}
