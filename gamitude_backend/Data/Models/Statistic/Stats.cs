using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace gamitude_backend.Models
{
    [BsonIgnoreExtraElements]
    public class Stats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("userId")]
        public string userId { get; set; }

        [BsonElement("strength")]
        public long strength { get; set; } = 0;

        [BsonElement("intelligence")]
        public long intelligence { get; set; } = 0;

        [BsonElement("fluency")]
        public long fluency { get; set; } = 0;

        [BsonElement("creativity")]
        public long creativity { get; set; } = 0;


    }

}