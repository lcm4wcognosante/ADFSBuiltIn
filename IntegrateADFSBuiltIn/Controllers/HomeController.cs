using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrateADFSBuiltIn.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
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