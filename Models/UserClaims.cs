using System;
using System.Collections.Generic;
using System.Security.Claims;
using Test.Interface;

namespace Test.Models
{
    public class UserClaims
    {
        public Lazy<IEnumerable<Claim>> claims { get; set;} = new Lazy<IEnumerable<Claim>>(() => generateClaims());

        private static IEnumerable<Claim> generateClaims()
        {
            yield return new Claim(ClaimTypes.NameIdentifier, "ssddff");
            yield return new Claim(ClaimTypes.Name, "Frank");
        }
    }
}