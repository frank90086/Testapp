using System;
using Test.Interface;

namespace Test.Models
{
    public class SecondDI : ISecondDI
    {
        public SecondDI(){}

        public string ReturnString(string s) => $"return string by use {s}";
    }
}