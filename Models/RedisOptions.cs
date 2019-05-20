using System;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Test.Models
{
    public class RedisOptions : IOptions<RedisOptions>
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public ConfigurationOptions ConfigurationOptions { get; set; }

        RedisOptions IOptions<RedisOptions>.Value { get {return this;} }
    }
}