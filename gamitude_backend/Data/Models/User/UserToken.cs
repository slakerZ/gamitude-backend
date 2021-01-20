using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class UserToken 
    {// todo change to is4
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }

        [BsonElement("token")]
        public string token { get; set; }

        [BsonElement("dateExpires")]
        public DateTime dateExpires { get; set; }

}


}