using LearnApi.COR.IRepository;

namespace LearnApi.COR
{
    public interface IUnitOfWork:IDisposable
    {
        ICustomerRepository CustomerRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IUserRepository UserRepository { get; }
        IDriverRepository DriverRepository { get; }
        IDriverProfileRepository DriverProfileRepository { get; }
        Task CompleteAsync();
    }
}
