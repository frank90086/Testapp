using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Test.Enricher;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => {
                    var env = hostingContext.HostingEnvironment;
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("serilog.json", false, true)
                        .AddJsonFile("serilog.{env.EnvironmentName}.json", true)
                        .AddEnvironmentVariables();
                    var config = builder.Build();

                    loggerConfiguration.ReadFrom.Configuration(config)
                        .AddCustomEnrichers(config);
                });
                // .UseKestrel(options =>
                // {
                //     options.Listen(IPAddress.Loopback, 8081);
                //     options.Listen(IPAddress.Loopback, 5443, listenOptions =>
                //     {
                //         listenOptions.UseHttps("localhost.pfx", "mangopaper892");
                //     });
                // });
    }
}
