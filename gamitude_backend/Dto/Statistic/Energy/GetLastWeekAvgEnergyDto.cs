using System;
using Microsoft.Extensions.Options;
using gamitude_backend.Settings;
using gamitude_backend.Configuration;

namespace gamitude_backend.Dto.Energy
{
    public class GetLastWeekAvgEnergyDto
    {
        public int emotions { get; set; }

        public int soul { get; set; }

        public int body { get; set; }

        public int mind { get; set; }
        public int dayCount;
        /// <summary>
        /// Calculates the avg adds rest of the week as Max if empty
        /// </summary>
        public GetLastWeekAvgEnergyDto weekAvg()
        {
            this.emotions = (this.emotions + ((7 - this.dayCount) * StaticValues.workDayLength)) / 7;
            this.soul = (this.soul + ((7 - this.dayCount) * StaticValues.workDayLength)) / 7;
            this.body = (this.body + ((7 - this.dayCount) * StaticValues.workDayLength)) / 7;
            this.mind = (this.mind + ((7 - this.dayCount) * StaticValues.workDayLength)) / 7;
            return this;
        }
        public GetLastWeekAvgEnergyDto scaleToPercent()
        {
            this.body = (this.body * 100) / StaticValues.workDayLength;
            this.soul = (this.soul * 100) / StaticValues.workDayLength;
            this.emotions = (this.emotions * 100) / StaticValues.workDayLength;
            this.mind = (this.mind * 100) / StaticValues.workDayLength;
            return this;
        }
    }
}
