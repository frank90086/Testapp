using Test.Interface;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Test.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context,  IFirstDI first)
        {
            // first.Name = "Changed By Middleware";
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error");
            }
        }
    }
}