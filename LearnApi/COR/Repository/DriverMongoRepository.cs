using LearnApi.COR.IRepository;
using LearnApi.Data;
using LearnApi.Entity;
using LearnApi.Helper;

namespace LearnApi.COR.Repository
{
    public class DriverMongoRepository : GenericRepositoryMongo<Driver>, IDriverRepository
    {
        public DriverMongoRepository(MongoDBConnection mongoContext,ILogger<GenericRepositoryMongo<Driver>>logger) 
            : base(mongoContext, logger)
        {
        }
    }
}
