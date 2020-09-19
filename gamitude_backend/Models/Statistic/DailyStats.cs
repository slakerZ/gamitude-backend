using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;
 
 
using gamitude_backend.Settings;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using gamitude_backend.Configuration;

namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class DailyStats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("strength")]
        public int Strength { get; set; }

        [BsonElement("intelligence")]
        public int Intelligence { get; set; }

        [BsonElement("fluency")]
        public int Fluency { get; set; }

        [BsonElement("creativity")]
        public int Creativity { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        public DailyStats validate()
        {
            if (this.Creativity > StaticValues.dayLenght) this.Creativity = StaticValues.dayLenght;
            else if (this.Creativity < 0) this.Creativity = 0;
            if (this.Fluency > StaticValues.dayLenght) this.Fluency = StaticValues.dayLenght;
            else if (this.Fluency < 0) this.Fluency = 0;
            if (this.Intelligence > StaticValues.dayLenght) this.Intelligence = StaticValues.dayLenght;
            else if (this.Intelligence < 0) this.Intelligence = 0;
            if (this.Strength > StaticValues.dayLenght) this.Strength = StaticValues.dayLenght;
            else if (this.Strength < 0) this.Strength = 0;
            return this;
        }

    }

}