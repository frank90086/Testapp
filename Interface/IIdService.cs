using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Test.Interface
{
    public interface IIdService
    {
        long GetIdBySnow();
    }
}