using LearnApi.COR.IRepository;
using LearnApi.Entity;
using LearnApi.Helper;

namespace LearnApi.COR.Repository
{
    public class DriverProfileMongoRespository : GenericRepositoryMongo<DriverProfile>, IDriverProfileRepository
    {
        public DriverProfileMongoRespository(MongoDBConnection mongoDBConnection, ILogger<GenericRepositoryMongo<DriverProfile>> logger) 
            : base(mongoDBConnection, logger)
        {
        }
    }
}
