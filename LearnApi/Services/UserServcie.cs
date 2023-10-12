using AutoMapper;
using LearnApi.COR;
using LearnApi.Entity;
using LearnApi.ExceptionHandler;
using LearnApi.Helper;
using LearnApi.Models.DTO;
using LearnApi.Servies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LearnApi.Services
{
    public class UserServcie : IUserService
    {
        public IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<IUserService> logger;
        private readonly JwtHelper jwtUtils;
        public UserServcie(IUnitOfWork unitOfWork, IMapper mapper, ILogger<IUserService> logger,JwtHelper jwtUtils)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.jwtUtils = jwtUtils;
        }

        public async Task<LoginResponseDto> Login(string username, string password)
        {
            LoginResponseDto responseDto = new LoginResponseDto();
            var existingUser = await unitOfWork.UserRepository.FindUserByEmail(username);
            if (existingUser == null)
            {
                throw new DataNotFoundException($"User '{username}' not found.");
            }
            if(existingUser.Password==password)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,username),
                    new Claim(ClaimTypes.Email,username),
                };
                var tokenDescriptor = jwtUtils.GetToken(authClaims);
                RefreshToken refreshToken = await unitOfWork.RefreshTokenRepository.GetAsync(token => token.User.Email == username);
                try
                {
                    if (refreshToken == null)
                    {
                        RefreshToken newRefreshToken = new RefreshToken
                        {
                            User = existingUser,
                            Refreshtoken = jwtUtils.GetRefreshToken(),
                            ValidTo=DateTime.Now,
                        };
                        await unitOfWork.RefreshTokenRepository.CreateAsync(newRefreshToken);
                        await unitOfWork.CompleteAsync();
                        responseDto.refreshToken = newRefreshToken.Refreshtoken;
                    }
                    else
                    {
                        refreshToken.ValidTo= DateTime.Now.AddDays(1);
                        await unitOfWork.CompleteAsync();
                        responseDto.refreshToken = refreshToken.Refreshtoken;
                    }
                    
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Error Generating token",ex);
                }
                responseDto.email = username;
                responseDto.tokenValidTo = tokenDescriptor.ValidTo;
                responseDto.authToken= new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
                var result=await unitOfWork.DriverRepository.FindByIdAsync("64f08cdb0307040d45898ecb");
                Console.WriteLine(result);
                return responseDto;
            }
            throw new ValidationException($"User '{username}' incorrect password.");

        }

        public async Task<LoginResponseDto> Register(UserRegisterDto userRegisterDto)
        {
            var existingUser = await unitOfWork.UserRepository.FindUserByEmail(userRegisterDto.Email);
            if (existingUser == null)
            {
                User newUser=mapper.Map<User>(userRegisterDto); 
                await unitOfWork.UserRepository.CreateAsync(newUser);
                await unitOfWork.CompleteAsync();
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userRegisterDto.Name),
                    new Claim(ClaimTypes.Email,userRegisterDto.Email),
                };
                var tokenDescriptor = jwtUtils.GetToken(authClaims);
                RefreshToken newRefreshToken = new RefreshToken
                {
                    User = existingUser,
                    Refreshtoken = jwtUtils.GetRefreshToken(),
                    ValidTo = DateTime.Now,
                };
                await unitOfWork.RefreshTokenRepository.CreateAsync(newRefreshToken);
                await unitOfWork.CompleteAsync();
                LoginResponseDto responseDto = new LoginResponseDto {
                    refreshToken = newRefreshToken.Refreshtoken,
                    email = userRegisterDto.Email,
                    authToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                    tokenValidTo= tokenDescriptor.ValidTo,
                };
                return responseDto;
            }
            throw new ValidationException($"User with'{userRegisterDto.Email}' already exists");
        }

        public async Task<LoginResponseDto> RefreshToken(string authToken, string refreshToken)
        {
            if (authToken is null || refreshToken is null)
                throw new ValidationException("Fill Required fields");
            var principal = jwtUtils.GetPrincipalFromExpiredToken(authToken);
            var username = principal.Identity.Name;
            var existingUser= await unitOfWork.UserRepository.GetAsync(u=>u.Email == username);
            if(existingUser is null)
                throw new ValidationException($"Invalid authToken");
            var oldRefreshToken = await unitOfWork.RefreshTokenRepository.GetAsync(token=>token.Refreshtoken==refreshToken);
            if (oldRefreshToken is null)
                throw new ValidationException("Wrong Refresh Token");
            if (oldRefreshToken.ValidTo <= DateTime.Now)
                throw new ValidationException("Token Expired");
            var tokenDescriptor = jwtUtils.GetToken(principal.Claims);
            RefreshToken newRefreshToken = new RefreshToken
            {
                User = existingUser,
                Refreshtoken = jwtUtils.GetRefreshToken(),
                ValidTo = DateTime.Now,
            };
            await unitOfWork.RefreshTokenRepository.CreateAsync(newRefreshToken);
            await unitOfWork.CompleteAsync();
            LoginResponseDto responseDto = new LoginResponseDto
            {
                refreshToken = newRefreshToken.Refreshtoken,
                email = username,
                authToken = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor),
                tokenValidTo = tokenDescriptor.ValidTo,
            };
            return responseDto;

        }
    }
}
