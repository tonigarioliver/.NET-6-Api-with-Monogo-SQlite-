using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LearnApi.Entity
{
    public class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
