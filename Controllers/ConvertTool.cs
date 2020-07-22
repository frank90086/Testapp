

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
        public ConvertToolController() { }

        public IActionResult Sign([FromQuery] string token)
        {
            var raw = string.Empty;
            var unixtime = MethodExtension.GetTimestamp();

            using (var stream = new StreamReader(HttpContext.Request.Body))
            {
                raw = stream.ReadToEnd();
            }

            var sign = ASEService.ToMD5($"{raw}{unixtime}{token}").ToLower();

            var result = new Result
                         {
                             Token = token,
                             UnixTime = unixtime.ToString(),
                             Sign = sign
                         };
            
            return new JsonResult(result);
        }
    }

    public class Result
    {
        public string Token { get; set; }
        public string UnixTime { get; set; }
        public string Sign { get; set; }
    }
}