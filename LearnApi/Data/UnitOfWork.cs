using LearnApi.COR;
using LearnApi.COR.IRepository;
using LearnApi.COR.IRespository;
using LearnApi.COR.Repository;
using LearnApi.Entity;
using LearnApi.Helper;

namespace LearnApi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _dbContext;
        private ILogger<IUnitOfWork> logger;

        // Define los campos privados para los repositorios
        private ICustomerRepository _customerRepository;
        private IUserRepository _userRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IDriverRepository _driverRepository;
        private IDriverProfileRepository _profileRepository;

        public UnitOfWork(
            ApiDbContext dbContext,
            MongoDBConnection mongoContext,
            ILogger<IUnitOfWork> logger,
            ILogger<GenericRepository<Customer>> logger1,
            ILogger<GenericRepository<User>> logger2,
            ILogger<GenericRepository<RefreshToken>> logger3,
            ILogger<GenericRepositoryMongo<Driver>>logger4,
            ILogger<GenericRepositoryMongo<DriverProfile>> logger5)
        {
            _dbContext = dbContext;
            this.logger = logger;

            // Inicializa los repositorios en el constructor
            _customerRepository = new CustomerRepository(dbContext, logger1);
            _userRepository = new UserRepository(dbContext, logger2);
            _refreshTokenRepository = new RefreshTokenRepository(_dbContext, logger3);
            _driverRepository = new DriverMongoRepository(mongoContext, logger4);
            _profileRepository = new DriverProfileMongoRespository(mongoContext,logger5);
        }

        // Proporciona los repositorios a través de propiedades de solo lectura
        public ICustomerRepository CustomerRepository => _customerRepository;
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
        public IUserRepository UserRepository => _userRepository;
        public IDriverRepository DriverRepository => _driverRepository;
        public IDriverProfileRepository DriverProfileRepository => _profileRepository;
        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }

}
