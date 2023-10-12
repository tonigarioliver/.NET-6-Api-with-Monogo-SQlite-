using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LearnApi.Entity
{
    public class Driver:MongoEntity
    {

        [BsonElement("Name")]
        public string DriverName { get; set; } = null!;

        public int Number { get; set; }
        public string Team { get; set; } = null!;
    }
}
