using gamitude_backend.Configuration;

namespace gamitude_backend.Dto.Energy
{
    public class GetDailyEnergyDto
    {
        public int emotions { get; set; }  = StaticValues.workDayLength;

        public int soul { get; set; } = StaticValues.workDayLength;

        public int body { get; set; } = StaticValues.workDayLength;

        public int mind { get; set; } = StaticValues.workDayLength;
        public GetDailyEnergyDto scaleToPercent()
        {
            this.body = (this.body * 100) / StaticValues.workDayLength;
            this.soul = (this.soul * 100) / StaticValues.workDayLength;
            this.emotions = (this.emotions * 100) / StaticValues.workDayLength;
            this.mind = (this.mind * 100) / StaticValues.workDayLength;
            return this;
        }
    }
}
