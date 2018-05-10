using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pjs1.Main.Models;
using Microsoft.Extensions.Options;

namespace Pjs1.Main.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpGet("/Error")]
        public IActionResult ErrorStatusCode()
        {
            var executeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var handlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            //ViewData["ErrorMessage"] = handlerFeature.Error.Message;
            //var requestFeature = HttpContext.Features.Get<IHttpRequestFeature>(); 
            //ViewData["ErrorUrl"] = handlerFeature.InvokeProperty<string>("Path");

            var statusCode = HttpContext.Response.StatusCode;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/Error/{statusCode}")]
        public IActionResult ErrorNotFound(int statusCode)
        {
            var executeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            ViewData["ErrorUrl"] = executeFeature?.OriginalPath;


            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
