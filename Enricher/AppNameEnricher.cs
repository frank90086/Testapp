using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Murmur;
using Serilog.Core;
using Serilog.Events;
using Test.Interface;

namespace Test.Enricher
{
    /// <summary>
    /// 新增HashTag Property 識別代號
    /// </summary>
    public class AppNameEnricher : ICustomEnricher
    {
        private readonly IConfiguration _config;
        public AppNameEnricher(IConfiguration config)
        {
            _config = config;
        }
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var appNAme = _config.GetSection("AppSetting");
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("APP", appNAme["AppName"]));
        }
    }
}