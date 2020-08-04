// using System;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.Extensions.Options;
 
 
// using gamitude_backend.Settings;

// namespace gamitude_backend.Models
// {
//     public class DailyStats
//     {
//         public int Id { get; set; }

//         public string UserId { get; set; }

//         public int Strength { get; set; }

//         public int Intelligence { get; set; }

//         public int Fluency { get; set; }

//         public int Creativity { get; set; }

//         public DateTime Date { get; set; }

//         // public DailyStats validate()
//         // {
//         //     if (this.Creativity > StaticValues.dayLenght) this.Creativity = StaticValues.dayLenght;
//         //     else if (this.Creativity < 0) this.Creativity = 0;
//         //     if (this.Fluency > StaticValues.dayLenght) this.Fluency = StaticValues.dayLenght;
//         //     else if (this.Fluency < 0) this.Fluency = 0;
//         //     if (this.Intelligence > StaticValues.dayLenght) this.Intelligence = StaticValues.dayLenght;
//         //     else if (this.Intelligence < 0) this.Intelligence = 0;
//         //     if (this.Strength > StaticValues.dayLenght) this.Strength = StaticValues.dayLenght;
//         //     else if (this.Strength < 0) this.Strength = 0;
//         //     return this;
//         // }

//     }

// }