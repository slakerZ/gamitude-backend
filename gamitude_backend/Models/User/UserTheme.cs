
 

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace gamitude_backend.Models 
{

    [BsonIgnoreExtraElements]
    public class UserTheme
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }
                 
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String userId { get; set; }

        [BsonElement("themeId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public String themeId { get; set; }

    }

}