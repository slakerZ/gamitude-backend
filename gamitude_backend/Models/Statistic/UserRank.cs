
 

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gamitude_backend.Models 
{

    [BsonIgnoreExtraElements]
    public class UserRank
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
                 
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("rankId")]
        public string RankId { get; set; }

    }

}