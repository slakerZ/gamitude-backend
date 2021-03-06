using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{


    [BsonIgnoreExtraElements]
    public class Rank
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
        public string imageUrl { get; set; }

        [BsonElement("rookie")]
        public bool rookie { get; set; }

        [BsonElement("style")]
        public GAMITUDE_STYLE Style { get; set; }

        [BsonElement("tier")]
        public RANK_TIER Tier { get; set; }//TODO Migrate to enum

        [BsonElement("dominant")]
        public RANK_DOMINANT Dominant { get; set; }//TODO Migrate to enum

    }


}