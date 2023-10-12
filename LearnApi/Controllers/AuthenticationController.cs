using LearnApi.Entity;
using LearnApi.Models;
using LearnApi.Models.DTO;
using LearnApi.Services;
using LearnApi.Servies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService userService;
        public AuthenticationController(IUserService userService) 
        {
            this.userService = userService;
        }
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromQuery] string userName, [FromQuery] string password)
        {
            return await userService.Login(userName, password);
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] UserRegisterDto newUser)
        {
            return await userService.Register(newUser);
        }
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<LoginResponseDto> Refresh([FromQuery] string authToken,[FromQuery] string refreshToken)
        {
            return await userService.RefreshToken(authToken, refreshToken);
        }

    }
}
