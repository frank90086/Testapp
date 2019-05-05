using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration  config)
    {
       _config = config;
    }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test(){
            string baseNamespace = "Test.Models";
            // string className = _config["SetModel:ClassName"];
            string className = Environment.GetEnvironmentVariable("classname");
            Type type = Type.GetType($"{baseNamespace}.{className}");
            BaseModel result = (Activator.CreateInstance(type)) as BaseModel;
            return View(model: result);
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
