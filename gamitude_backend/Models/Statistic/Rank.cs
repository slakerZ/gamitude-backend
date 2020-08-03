
 

using System;
 

namespace gamitude_backend.Models 
{

    public enum GAMITUDE_STYLE
    {
        DEFAULT,WINTER,BUSINESS,LOL
    }
    public enum RANK_TIER
    {
        F,D,C,B,A,S
    }
    public enum RANK_DOMINANT
    {
        STRENGHT,INTELLIGENCE,FLUENCY,CREATIVITY,BALANCED 

    }
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