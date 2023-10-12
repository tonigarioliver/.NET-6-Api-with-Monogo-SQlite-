using LearnApi.Config;
using LearnApi.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Runtime.InteropServices;

namespace LearnApi.Helper
{
    public class MongoDBConnection
    {
        private readonly IMongoDatabase mongoContext;

        public MongoDBConnection(IOptions<MongoDBConfig> databaseSettings)
        {
            MongoDBConfig config = databaseSettings.Value;
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            mongoContext = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        }

        public IMongoDatabase context => mongoContext;
    }
}
