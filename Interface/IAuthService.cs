using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Test.Interface
{
    public interface IAuthService
    {
        Task<bool> CheckClientCredential(string id, string password);
    }
}