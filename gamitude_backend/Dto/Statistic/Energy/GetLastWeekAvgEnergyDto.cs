using System;
using Microsoft.Extensions.Options;
using gamitude_backend.Settings;

namespace gamitude_backend.Dto.Energy
{
    public class GetLastWeekAvgEnergyDto
    {
        public int Emotions { get; set; }

        public int Soul { get; set; }

        public int Body { get; set; }

        public int Mind { get; set; }
        public int dayCount;
        /// <summary>
        /// Calculates the avg adds rest of the week as Max if empty
        /// </summary>
        public GetLastWeekAvgEnergyDto weekAvg()
        {
            this.Emotions = (this.Emotions + ((7 - this.dayCount) * StaticValues.dayLenght)) / 7;
            this.Soul = (this.Soul + ((7 - this.dayCount) * StaticValues.dayLenght)) / 7;
            this.Body = (this.Body + ((7 - this.dayCount) * StaticValues.dayLenght)) / 7;
            this.Mind = (this.Mind + ((7 - this.dayCount) * StaticValues.dayLenght)) / 7;
            return this;
        }
        public GetLastWeekAvgEnergyDto scaleToPercent()
        {
            this.Body = (this.Body * 100) / StaticValues.dayLenght;
            this.Soul = (this.Soul * 100) / StaticValues.dayLenght;
            this.Emotions = (this.Emotions * 100) / StaticValues.dayLenght;
            this.Mind = (this.Mind * 100) / StaticValues.dayLenght;
            return this;
        }
    }
}
