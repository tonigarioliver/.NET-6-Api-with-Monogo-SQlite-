using MongoDB.Bson.Serialization.Attributes;

namespace LearnApi.Entity
{
    public class DriverProfile : MongoEntity
    {

        [BsonElement("Name")]
        public string DriverName { get; set; } = null!;

        public int Number { get; set; }
        public string Team { get; set; } = null!;

        public byte[] DriverImage { get; set; }
    }
}
