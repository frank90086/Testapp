using System;
using System.Text;
using Murmur;
using Serilog.Core;
using Serilog.Events;
using Test.Interface;

namespace Test.Enricher
{
    /// <summary>
    /// 新增HashTag Property 識別代號
    /// </summary>
    public class EnvironmentNameEnricher : ICustomEnricher
    {
        private static Lazy<string> evnName { get; } = new Lazy<string>(() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Null");
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("EVN", evnName.Value));
        }
    }
}