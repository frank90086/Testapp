using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Test.Interface;
using Test.Extension;

namespace Test.Models
{
    public class MiddleObject : IMiddleObject
    {
        private IConfiguration _config;
        private readonly string _readonly;
        private readonly string _writeonly;
        private ISecondDI _second;
        private string _currentString;
        public MiddleObject(IConfiguration config, ISecondDI second)
        {
            _config = config;
            _readonly = _config.GetConnectionString("ReadOnly");
            _writeonly = _config.GetConnectionString("WriteOnly");
            _currentString = _readonly;
            _second = second;
        }

        public string ReturnString()
        {
            CheckMethod();
            return _second.ReturnString(_currentString);
        }

        private void CheckMethod()
        {
            StackTrace stack = new StackTrace();
            string calledMethodName = stack.GetFrame(2).GetMethod().Name;
            if (calledMethodName.Contains("Post", StringComparison.CurrentCultureIgnoreCase))  _currentString = _writeonly;
        }
    }
}