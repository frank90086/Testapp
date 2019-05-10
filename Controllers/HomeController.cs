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
using Test.Interface;
using Microsoft.Extensions.Caching.Distributed;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private IFirstDI _first;
        private IDistributedCache _cache;

        public HomeController(IConfiguration  config, IFirstDI first, IDistributedCache cache)
    {
       _config = config;
       _first = first;
       _cache = cache;
    }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test(){
            string baseNamespace = "Test.Models";
            string className = _config["SetModel:ClassName"];
            // string className = Environment.GetEnvironmentVariable("classname");
            Type type = Type.GetType($"{baseNamespace}.{className}");
            BaseModel result = (Activator.CreateInstance(type)) as BaseModel;
            return View(model: result);
        }

        public IActionResult TestDI(){
            ViewBag.GetResult = _first.GetString();
            ViewBag.PostResult = _first.PostString();
            return View();
        }

        [HttpGet]
        public IActionResult SetRedis(){
            return View();
        }

        [HttpPost]
        public IActionResult SetRedis(string key, string value){
            if (String.IsNullOrEmpty(key)) _cache.SetString(key, value);
            return RedirectToAction("GetRedis");
        }

        [HttpGet]
        public IActionResult GetRedis(){
            return View();
        }

        [HttpPost]
        public IActionResult GetRedis(string key){
            if (!String.IsNullOrEmpty(key)){
                var value = _cache.GetString(key);
                ViewBag.GetValue = value;
            }
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
