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
    public class HashEnricher : ICustomEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var murmur = MurmurHash.Create32();
            var bytes = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text);
            var hash = murmur.ComputeHash(bytes);
            var numericHash = BitConverter.ToUInt32(hash, 0);
            var eventId = propertyFactory.CreateProperty("HashTag", numericHash);
            logEvent.AddPropertyIfAbsent(eventId);
        }
    }
}