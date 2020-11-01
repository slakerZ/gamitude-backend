
 

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace gamitude_backend.Models 
{

    [BsonIgnoreExtraElements]
    public class UserRanks
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }
                 
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String userId { get; set; }

        [BsonElement("rankIds")]
        [BsonRepresentation(BsonType.ObjectId)]
        public List<String> rankIds { get; set; } = new List<String>();

    }

}