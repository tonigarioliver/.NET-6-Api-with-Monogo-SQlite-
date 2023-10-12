
using LearnApi.COR;
using LearnApi.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace LearnApi.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUnitOfWork unitOfWork;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUnitOfWork unitOfWork) : base(options, logger, encoder, clock)
        {
            this.unitOfWork = unitOfWork;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No header found");
            }
            var headervalue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (headervalue != null)
            {
                var bytes = Convert.FromBase64String(headervalue.Parameter);
                string credentials = Encoding.UTF8.GetString(bytes);
                string[] array = credentials.Split(":");
                string username = array[0];
                string password = array[1];
                var user = await unitOfWork.UserRepository.GetAsync(item => item.Email == username && item.Password == password);
                if (user != null)
                {
                    var claim = new[] { new Claim(ClaimTypes.Email, user.Email) };
                    var identity = new ClaimsIdentity(claim, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                else
                {
                    return AuthenticateResult.Fail("UnAutorized");
                }
            }
            else
            {
                return AuthenticateResult.Fail("Empty header");
            }
        }
    }
}
