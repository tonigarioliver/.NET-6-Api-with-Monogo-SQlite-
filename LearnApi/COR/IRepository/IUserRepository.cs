using LearnApi.COR.IRespository;
using LearnApi.COR.Repository;
using LearnApi.Entity;

namespace LearnApi.COR.IRepository
{
    public interface IUserRepository:IGenericRepository<User>
    {
        public Task<User> FindUserByEmail(string email);
    }
}
