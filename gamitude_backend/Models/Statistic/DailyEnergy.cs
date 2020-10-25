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
        public string id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("mind")]
        public int mind { get; set; }

        [BsonElement("soul")]
        public int soul { get; set; }

        [BsonElement("body")]
        public int body { get; set; }

        [BsonElement("emotions")]
        public int emotions { get; set; }

        [BsonElement("dateCreated")]
        public DateTime dateCreated { get; set; }
        public DailyEnergy init()
        {
            this.body = StaticValues.workDayLength;
            this.emotions = StaticValues.workDayLength;
            this.soul = StaticValues.workDayLength;
            this.mind = StaticValues.workDayLength;
            return this;
        }
        public DailyEnergy validate()
        {
            if (this.body > StaticValues.workDayLength) this.body = StaticValues.workDayLength;
            else if (this.body < 0) this.body = 0;
            if (this.soul > StaticValues.workDayLength) this.soul = StaticValues.workDayLength;
            else if (this.soul < 0) this.soul = 0;
            if (this.emotions > StaticValues.workDayLength) this.emotions = StaticValues.workDayLength;
            else if (this.emotions < 0) this.emotions = 0;
            if (this.mind > StaticValues.workDayLength) this.mind = StaticValues.workDayLength;
            else if (this.mind < 0) this.mind = 0;
            return this;
        }
    }

}