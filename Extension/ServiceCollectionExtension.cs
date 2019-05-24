using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.Interface;
using Test.Models;

namespace Test.Extension{
    public static class ServiceCollectionExtension{
        public static IServiceCollection AddRedis(this IServiceCollection service, Action<RedisOptions> fun)
        {
            service.AddOptions();
            service.Configure(fun);
            service.Add(ServiceDescriptor.Singleton<IRedisContext, RedisContext>());

            return service;
        }

        public static IServiceCollection AddModuelConfig(this IServiceCollection service, IConfiguration config)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var typies = assembly.GetTypes().Where(p => p.GetInterfaces().Contains(typeof(IModuelConfig)));
                foreach (var type in typies)
                {
                    var instance = (IModuelConfig)Activator.CreateInstance(type);
                    instance.Set(service, config);
                }
            }

            return service;
        }
    }
}