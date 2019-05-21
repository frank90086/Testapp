using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Test.Interface;
using Test.Models;

namespace Test.Service
{
    public class TokenService : ITokenService
    {
        public AuthenticationTicket GetToken()
        {
            UserClaims user = new UserClaims();
            var identity = new ClaimsIdentity(user.claims.Value, nameof(TokenService));
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), "Test.App");
            return ticket;
        }

        public (string id, string name) GetUser(string token)
        {
            var credentialBytes = Convert.FromBase64String(token);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var id = credentials[0];
            var name = credentials[1];
            return (id, name);
        }
    }
}