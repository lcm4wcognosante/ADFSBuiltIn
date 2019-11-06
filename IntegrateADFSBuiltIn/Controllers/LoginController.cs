using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrateADFSBuiltIn.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IntegrateADFSBuiltIn.Controllers
{
    // this is the web version of the mobile version in:
    // https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-backend-how-to-use-server-sdk#custom-auth

    //[ApiController]
    public class LoginController : Controller
    {
        public IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        //[HttpPost]
        //Route login/login
        //public IActionResult Login([FromBody] JObject assertion)
        public IActionResult Login([FromBody] JObject assertion)
        {
            var headers = string.Join("<br>",
                Request.Headers.Keys.Select(
                    key => string.Format($"<b>Key:</b>{key}, <b>Value:</b>{Request.Headers[key]}")
            ));
            ViewBag.Headers = headers;

            return View();
        }
    }
}
