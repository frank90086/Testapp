using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                    var evn = hostingContext.HostingEnvironment;
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(evn.ContentRootPath)
                        .AddJsonFile("serilog.json", false, true)
                        .AddEnvironmentVariables();
                    var config = builder.Build();

                    loggerConfiguration.ReadFrom.Configuration(config)
                        .AddCustomEnrichers(config);
                });
    }
}
