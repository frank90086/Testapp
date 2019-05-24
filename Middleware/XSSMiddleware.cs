using Test.Interface;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Test.XSS;
using System.Text;

namespace Test.Middleware
{
    public class XSSMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly XSSOptions _options;

        public XSSMiddleware(RequestDelegate next, XSSOptions options)
        {
            _next = next;
            _options = options;
        }

        private string Header => _options.ReadOnly
        ? "Content-Security-Policy-Report-Only" : "Content-Security-Policy";

        private string HeaderValue
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(_options.Defaults);
                stringBuilder.Append(_options.Connects);
                stringBuilder.Append(_options.Fonts);
                stringBuilder.Append(_options.Frames);
                stringBuilder.Append(_options.Images);
                stringBuilder.Append(_options.Media);
                stringBuilder.Append(_options.Objects);
                stringBuilder.Append(_options.Scripts);
                stringBuilder.Append(_options.Styles);
                stringBuilder.Append(_options.FrameAncestors);
                if (!string.IsNullOrEmpty(_options.ReportURL))
                    stringBuilder.Append($"report-uri {_options.ReportURL};");
              return stringBuilder.ToString();
            }
        }

        public async Task Invoke(HttpContext context,  IFirstDI first)
        {
            context.Response.Headers.Add(Header, HeaderValue);
            if (!string.IsNullOrEmpty(_options.FrameAncestors.XFrame))
                context.Response.Headers.Add("X-Frame-Options", _options.FrameAncestors.XFrame);
            await _next(context);
        }
    }
}