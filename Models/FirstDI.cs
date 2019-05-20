using System;
using System.Threading;
using System.Threading.Tasks;
using Test.Interface;

namespace Test.Models
{
    public class FirstDI : IFirstDI
    {
        IMiddleObject _middle;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        public FirstDI(IMiddleObject middle){
            _middle = middle;
        }

        public string GetString() => _middle.ReturnString();

        public string PostString() => _middle.ReturnString();

        public async Task<string> GetStringAsync(string value)
        {
            string _str = string.Empty;
            ISecondDI _second = _middle.ReturnSecondDI();
            _str = $@"Thread ID : {Thread.CurrentThread.ManagedThreadId}, Message : {value}/n
            Instance HashCode : {this.GetHashCode()}";
            return $"{_str}, {_second.GetHashCode()}";
        }
    }
}