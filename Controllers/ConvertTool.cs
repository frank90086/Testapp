

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Extension;
using Test.Service;

namespace Test.Controllers
{
    [AllowAnonymous]
    public class ConvertToolController : ControllerBase
    {
        public ConvertToolController()
        {

        }

        public IActionResult Sign([FromQuery] string token)
        {
            var raw = string.Empty;
            var unixtime = MethodExtension.GetTimestamp();
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                raw = stream.ReadToEnd();
            }
            var result = ASEService.ToMD5($"{raw}{unixtime}{token}").ToLower();
            return Content($"Token:{token}\r\nUnixtime:{unixtime}\r\nSign:{result}");
        }
    }
}