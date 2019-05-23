

using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{
    public class OAuthController : ControllerBase
    {
        public OAuthController()
        {

        }

        public IActionResult SayHi()
        {
            return Content("Hi");
        }

        public IActionResult Token()
        {
            var modelToken = HttpContext.User.Claims.Single(t => t.Type == "Token").Value;
                    return Ok(new {access_token = modelToken});
        }
    }
}