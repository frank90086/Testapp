using System;
using System.Threading;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Test.Interface;

namespace Test.Models
{
    public class RedisContext : IRedisContext
    {
        private ConnectionMultiplexer _con;
        private IDatabase _redis;
        private IDatabase Redis{
            get {
                Connect();
                return _redis;
            }
        }
        private RedisOptions _options;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        public RedisContext(IOptions<RedisOptions> options){
            _options = options.Value;
        }

        public void SetValue(string key, string value){
            if (!String.IsNullOrEmpty(key)){
                Redis.StringSet(key, value);
            }
        }

        public string GetValue(string key){
            if (!String.IsNullOrEmpty(key)){
                return Redis.StringGet(key);
            }
            return string.Empty;
        }

        private void Connect(){
            if(_redis != null) return;

            _connectionLock.Wait();
            try
            {
                if(_redis == null){
                    if(_options.ConfigurationOptions != null)
                        _con = ConnectionMultiplexer.Connect(_options.ConfigurationOptions);
                    else
                        _con = ConnectionMultiplexer.Connect(_options.ConnectionString);
                    _redis = _con.GetDatabase();
                }
            }
            finally{
                _connectionLock.Release();
            }
        }
    }
}