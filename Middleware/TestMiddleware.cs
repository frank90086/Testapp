using Test.Interface;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Test.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,  IFirstDI first)
        {
            // first.Name = "Changed By Middleware";
            await _next(context);
        }
    }
}