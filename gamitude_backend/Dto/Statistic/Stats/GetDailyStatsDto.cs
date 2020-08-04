// using Microsoft.Extensions.Options;
// using gamitude_backend.Settings;

// namespace gamitude_backend.Dto.Stats
// {
//     public class GetDailyStatsDto
//     {

//         public int Strength { get; set; }

//         public int Intelligence { get; set; }

//         public int Fluency { get; set; }

//         public int Creativity { get; set; }

//         public GetDailyStatsDto scaleToPercent()
//         {
//             this.Creativity =(this.Creativity*100)/ StaticValues.dayLenght;
//             this.Fluency = (this.Fluency*100)/StaticValues.dayLenght;
//             this.Intelligence =(this.Intelligence*100)/ StaticValues.dayLenght;
//             this.Strength = (this.Strength *100)/StaticValues.dayLenght;
//             return this;
//         }
//     }
// }
