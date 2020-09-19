


using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models
{


    [BsonIgnoreExtraElements]
    public class Rank
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("style")]
        public GAMITUDE_STYLE Style { get; set; }

        [BsonElement("tier")]
        public RANK_TIER Tier { get; set; }//TODO Migrate to enum

        [BsonElement("dominant")]
        public RANK_DOMINANT Dominant { get; set; }//TODO Migrate to enum

        [BsonElement("image")]
        public string ImageUrl { get; set; }

    }


}