using LearnApi.COR.IRepository;
using LearnApi.Data;
using LearnApi.Entity;

namespace LearnApi.COR.Repository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApiDbContext dbContext,ILogger<GenericRepository<Customer>>logger) : base(dbContext,logger)
        {
        }
    }
}
