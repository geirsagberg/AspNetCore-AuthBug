using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthBug
{
    /// <summary>
    ///     Searches the 'Authorization' header for an 'ApiKey' field. If it is found, it is compared to the 'ApiKey' stored in
    ///     the current 'ApiKeyOptions'
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeySchemeOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeySchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
#pragma warning disable 1998
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var parsedValue))
            {
                return AuthenticateResult.NoResult();
            }

            if (!"ApiKey".Equals(parsedValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            var apiKey = parsedValue.Parameter;

            if (!Guid.TryParse(apiKey, out var apiKeyGuid))
            {
                return AuthenticateResult.Fail("Invalid API key format");
            }

            var isValid = apiKeyGuid == Options.ApiKey;

            if (!isValid)
            {
                return AuthenticateResult.Fail("Invalid API key");
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, "Auth0")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
