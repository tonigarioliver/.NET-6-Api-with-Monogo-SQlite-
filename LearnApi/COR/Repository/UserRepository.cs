using LearnApi.COR.IRepository;
using LearnApi.Data;
using LearnApi.Entity;

namespace LearnApi.COR.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApiDbContext dbContext,ILogger<GenericRepository<User>>logger) : base(dbContext,logger)
        {
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await GetAsync(user => user.Email == email);
        }
    }
}
