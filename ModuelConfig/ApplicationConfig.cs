using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Interface;
using Test.Models;
using Test.Service;

namespace Test.ModuelConfig
{
    public class ApplicationConfig : IModuelConfig
    {
        public void Set(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IFirstDI, FirstDI>();
            services.AddTransient<IMiddleObject, MiddleObject>();
            services.AddTransient<ISecondDI, SecondDI>();
            services.AddSingleton<IRegexRule, RegexRuleModel>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IUserRepository>(new UserRepository(config));
            services.AddSingleton<ISnowflake>(Snowflake.Instance(long.Parse(config["MachineId:Default"])));
            services.AddSingleton<IIdService, IdService>();
        }
    }
}