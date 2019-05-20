using System;
using System.Threading.Tasks;

namespace Test.Interface
{
    public interface IFirstDI
    {
        string GetString();
        string PostString();

        Task<string> GetStringAsync(string value);
    }
}