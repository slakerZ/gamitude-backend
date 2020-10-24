
 

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
        public String id { get; set; }
                 
        [BsonElement("userId")]
        public String userId { get; set; }

        [BsonElement("rankId")]
        public String rankId { get; set; }

    }

}