using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{


    [BsonIgnoreExtraElements]
    public class Theme
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("priceStrength")]
        public long priceStrength { get; set; }

        [BsonElement("priceIntelligence")]
        public long priceIntelligence { get; set; }

        [BsonElement("priceFluency")]
        public long priceFluency { get; set; }

        [BsonElement("priceCreativity")]
        public long priceCreativity { get; set; }

        [BsonElement("priceEuro")]
        public long priceEuro { get; set; }

        [BsonElement("image")]
        public string colorPallet { get; set; }

    }


}