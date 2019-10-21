using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrateADFSBuiltIn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IntegrateADFSBuiltIn.Controllers
{
    // this is the web version of the mobile version in:
    // https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-backend-how-to-use-server-sdk#custom-auth

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Login([FromBody] JObject assertion)
        {
            IActionResult response = Unauthorized();
            var user = ValidateUser(assertion);

            if (user != null)
            {
                var tokenString = GenerateJWT(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private object GenerateJWT(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User ValidateUser(JObject assertion)
        {
            User user = null;

            // hardcoded for testing purposes
            if (assertion["username"].ToString() == "ADFSUser")
            {
                user = new User() { Id = 1, UserName = "ADFSUser" };
            }

            return user;
        }
    }
}
