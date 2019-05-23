using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Test.Extension;

namespace Test.Interface
{
    public class OAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthService _auth;
        public OAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthService auth
        ) : base(options, logger, encoder, clock)
        {
            _auth = auth;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticationHeaderValue authHeader;
            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out authHeader))
                return AuthenticateResult.Fail("Get Out");

            var schema = authHeader.Scheme;
            var token = authHeader.Parameter;
            string[] credentials;

            if (schema.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                byte[] bytes = Convert.FromBase64String(token);
                credentials = Encoding.UTF8.GetString(bytes).Split(":");
                if (credentials.Length != 2)
                    return AuthenticateResult.Fail("Get Out");

                var clientId = credentials[0];
                var clientPassword = credentials[1];

                if (! await _auth.CheckClientCredential(clientId, clientPassword))
                    return AuthenticateResult.Fail("Get Out");

                string[] userArray = new string[]{clientId, clientPassword, MethodExtension.GetTimestamp().ToString()};
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, clientId),
                    new Claim(ClaimTypes.NameIdentifier, clientPassword),
                    new Claim("Token", MethodExtension.EncryptToken(userArray))
                };
                var identify = new ClaimsIdentity(claims, nameof(OAuthHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identify), Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            if (schema.Equals("Bearer", StringComparison.OrdinalIgnoreCase))
            {
                credentials = MethodExtension.DecryptToken(token).Split(':');
                if (credentials.Count() < 3)
                    return AuthenticateResult.Fail("Get Out");
                var clientId = credentials[0];
                var clientPassword = credentials[1];
                var timestamp = credentials[2];

                if (! await _auth.CheckClientCredential(clientId, clientPassword))
                    return AuthenticateResult.Fail("Get Out");

                if (!MethodExtension.CheckTimestamp(timestamp, 60))
                    return AuthenticateResult.Fail("Get Out");

                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, clientId),
                    new Claim(ClaimTypes.NameIdentifier, clientPassword)
                };
                var identify = new ClaimsIdentity(claims, nameof(OAuthHandler));
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identify), Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.NoResult();
        }
    }
}