using System;
using Microsoft.Extensions.Options;
using gamitude_backend.Settings;

namespace gamitude_backend.Dto.Energy
{
    public class GetDailyEnergyDto
    {
        public int Emotions { get; set; }  = StaticValues.dayLenght;

        public int Soul { get; set; } = StaticValues.dayLenght;

        public int Body { get; set; } = StaticValues.dayLenght;

        public int Mind { get; set; } = StaticValues.dayLenght;
        public GetDailyEnergyDto scaleToPercent()
        {
            this.Body = (this.Body * 100) / StaticValues.dayLenght;
            this.Soul = (this.Soul * 100) / StaticValues.dayLenght;
            this.Emotions = (this.Emotions * 100) / StaticValues.dayLenght;
            this.Mind = (this.Mind * 100) / StaticValues.dayLenght;
            return this;
        }
    }
}
