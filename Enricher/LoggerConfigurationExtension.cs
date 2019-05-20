using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Test.Interface;

namespace Test.Enricher
{
    /// <summary>
    /// Serilog 設定模組擴充
    /// </summary>
    public static class LoggerConfigurationExtension
    {
        /// <summary>
        /// 套用自訂的 <see cref="ILogEventEnricher"/>。讀取Assemblies中繼承<code>IcustomEnricher</code>組態檔，並加載進 serilog 設定。
        /// </summary>
        /// <param name="loggerConfiguration">Serilog 設定組態</param>
        /// <param name="config">設定檔組態</param>
        /// <returns></returns>
        public static LoggerConfiguration AddCustomEnrichers(this LoggerConfiguration loggerConfiguration, IConfiguration config)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                        .Where(p => p.GetInterfaces().Contains(typeof(ICustomEnricher)));

                foreach (var type in types)
                {
                    ICustomEnricher enricher = null;
                    var cons = type.GetConstructors();
                    if (cons.Any(t => t.GetParameters().Length > 0))
                    {
                        var pars = cons[0].GetParameters();
                        if (pars[0].Name == "config")
                            enricher = Activator.CreateInstance(type, config) as ICustomEnricher;
                    }
                    else
                        enricher = Activator.CreateInstance(type) as ICustomEnricher;
                    if (enricher != null)
                        loggerConfiguration.Enrich.With(enricher);
                }
            }

            return loggerConfiguration;
        }
    }
}