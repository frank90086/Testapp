using Microsoft.AspNetCore.Authentication;

namespace Test.Interface
{
    public interface ITokenService
    {
        AuthenticationTicket GetToken();
        (string id, string name) GetUser(string token);
    }
}