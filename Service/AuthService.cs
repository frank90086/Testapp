using System;
using System.Threading.Tasks;
using Test.Interface;

namespace Test.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _user;
        public AuthService(IUserRepository user)
        {
            _user = user;
        }

        public async Task<bool> CheckClientCredential(string id, string password)
        {
            return _user.CheckClientCredential(id, password);
        }
    }
}