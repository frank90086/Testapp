

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test.Extension;
using Test.Interface;
using Test.Service;

namespace Test.Controllers
{
    [AllowAnonymous]
    public class IdToolController : ControllerBase
    {
        private IIdService _idGenerator;
        public IdToolController(IIdService idGenerator)
        {
            _idGenerator = idGenerator;
        }

        public IActionResult GetBySnow([FromQuery]int count = 1)
        {
            var result = forloop(count);
            return Ok(result);
        }

        private IEnumerable<string> forloop(int count)
        {
            for (int i = 0; i < count; i++)
                yield return $"{_idGenerator.GetIdBySnow().ToString()}";
        }
    }
}