using LearnApi.COR.IRepository;
using LearnApi.COR.IRespository;
using LearnApi.Data;
using LearnApi.Entity;

namespace LearnApi.COR.Repository
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>,IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApiDbContext dbContext, ILogger<GenericRepository<RefreshToken>> logger) : base(dbContext, logger)
        {
        }
    }
}
