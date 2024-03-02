using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace CartManagmentSystem.Models
{
    /// <summary>
    /// Handler for basic authentication.
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// Constructor for BasicAuthenticationHandler class.
        /// </summary>
        /// <param name="options">Authentication scheme options.</param>
        /// <param name="logger">Logger instance.</param>
        /// <param name="encoder">UrlEncoder instance.</param>
        /// <param name="clock">SystemClock instance.</param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        ) : base(options, logger, encoder, clock)
        {

        }

        /// <summary>
        /// Handles authentication asynchronously.
        /// </summary>
        /// <returns>An AuthenticateResult representing the outcome of the authentication process.</returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header was not found");
            }
            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);

                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");

                string token = credentials[0];
                string key = credentials[1];

                if (!UserFunctions.GetAuthorizationDetails(token, key, out string name, out string id))
                {
                    return AuthenticateResult.Fail("Invalid username or password");
                }
                else
                {
                    Request.Headers.Add("CompanyName", name);
                    Request.Headers.Add("CompanyId", id);
                    var claims = new[] { new Claim(ClaimTypes.Name, name) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception)
            {

                return AuthenticateResult.Fail("Error has occurred");

            }
        }
    }
}
