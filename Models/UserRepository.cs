using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Test.Interface;

namespace Test.Models
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration _config;
        public Lazy<IConfigurationSection> configSection { get; set;}

        public UserRepository(IConfiguration config)
        {
            _config = config;
            configSection = new Lazy<IConfigurationSection>(() => _config.GetSection("ClientCredential"));
        }

        public bool CheckClientCredential(string id, string password)
        {
            var checkid = configSection.Value.GetValue<string>("ClientId");
            var checkpassword = configSection.Value.GetValue<string>("ClientPassword");
            if (id.Equals(checkid))
            {
                if (password.Equals(checkpassword))
                    return true;
            }
            return false;
        }
    }
}