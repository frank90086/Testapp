using System;
using System.Threading.Tasks;
using Test.Interface;

namespace Test.Service
{
    public class IdService : IIdService
    {
        private ISnowflake _snow;
        public IdService(ISnowflake snow)
        {
            _snow = snow;
        }

        public long GetIdBySnow() => _snow.GetId();
    }
}