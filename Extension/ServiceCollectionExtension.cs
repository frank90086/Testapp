using System;
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
    }
}