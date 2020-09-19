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
    public class DailyEnergy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("mind")]
        public int Mind { get; set; }

        [BsonElement("soul")]
        public int Soul { get; set; }

        [BsonElement("body")]
        public int Body { get; set; }

        [BsonElement("emotions")]
        public int Emotions { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }
        public DailyEnergy init()
        {
            this.Body = StaticValues.dayLenght;
            this.Emotions = StaticValues.dayLenght;
            this.Soul = StaticValues.dayLenght;
            this.Mind = StaticValues.dayLenght;
            return this;
        }
        public DailyEnergy validate()
        {
            if (this.Body > StaticValues.dayLenght) this.Body = StaticValues.dayLenght;
            else if (this.Body < 0) this.Body = 0;
            if (this.Soul > StaticValues.dayLenght) this.Soul = StaticValues.dayLenght;
            else if (this.Soul < 0) this.Soul = 0;
            if (this.Emotions > StaticValues.dayLenght) this.Emotions = StaticValues.dayLenght;
            else if (this.Emotions < 0) this.Emotions = 0;
            if (this.Mind > StaticValues.dayLenght) this.Mind = StaticValues.dayLenght;
            else if (this.Mind < 0) this.Mind = 0;
            return this;
        }
    }

}