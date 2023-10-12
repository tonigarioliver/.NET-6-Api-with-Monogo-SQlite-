using LearnApi.Models.DTO;

namespace LearnApi.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto> Login(string username, string password);
        Task<LoginResponseDto> Register(UserRegisterDto userRegisterDto);
        Task<LoginResponseDto> RefreshToken(string authToken, string refreshToken);
    }

}
