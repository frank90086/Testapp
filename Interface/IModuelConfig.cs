using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Test.Interface
{
    public interface IModuelConfig
    {
        void Set(IServiceCollection services, IConfiguration config);
    }
}