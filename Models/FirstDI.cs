using System;
using Test.Interface;

namespace Test.Models
{
    public class FirstDI : IFirstDI
    {
        IMiddleObject _middle;
        public FirstDI(IMiddleObject middle){
            _middle = middle;
        }

        public string GetString() => _middle.ReturnString();

        public string PostString() => _middle.ReturnString();
    }
}