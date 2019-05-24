

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Test.Interface;
using Test.Middleware;
using Test.XSS;


namespace Test.Extension{
    public static class ApplocationBuilderExtension{
        public static IApplicationBuilder AddXSS(this IApplicationBuilder app, Action<XSSOptions> fun)
        {
            var options = new XSSOptions();
            fun(options);
            return app.UseMiddleware<XSSMiddleware>(options);
        }
    }
}