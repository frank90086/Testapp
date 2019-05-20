using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Test.Interface;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private IFirstDI _first;
        private readonly IRedisContext _cache;
        private readonly IRegexRule _regexRule;
        // private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration config, IFirstDI first, IRedisContext cache, IRegexRule regexRule)
        {
            _config = config;
            _first = first;
            _cache = cache;
            _regexRule = regexRule;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            string baseNamespace = "Test.Models";
            string className = _config["SetModel:ClassName"];
            // string className = Environment.GetEnvironmentVariable("classname");
            Type type = Type.GetType($"{baseNamespace}.{className}");
            BaseModel result = (Activator.CreateInstance(type)) as BaseModel;
            return View(model: result);
        }

        public IActionResult TestDI()
        {
            ViewBag.GetResult = _first.GetString();
            ViewBag.PostResult = _first.PostString();
            return View();
        }

        public IActionResult TestRegex()
        {
            string sql = @"
               SELECT ms.id	    `MerchantStatementId`
                    , msa.id	`MerchantStatementAccountId`
                    , mst.id    `MerchantStatementTransferId`
                    FROM MerchantStatement ms
                    deletes update MerchantStatementAccount msa ON ms.id = msa.merchantStatementId
                    INNER JOIN MerchantStatementTransfer mst ON msa.id = mst.merchantStatementAccountId
                    insert mst.id IN @TransferIds
                      AND mst.dataState = @insertState
                      AND UpdateTime = @deletee
                      AND ms.dataState  = @insert ime; ";
            string sqlUpdate = @"
               update MerchantStatement
                    SET state            = @State,
                        verifyId         = @VerifyId,
                        verifyTime       = @VerifyTime,
                        verifyFailReason = select ailReason,
                        updaterId        = @UpdaterId,

                        upda delete       = @UpdateTime,
                        updateDate       = @Up update ate
                    WHERE id        = @Id
                      AND dataState = @DataState; insert";
            string patten = _regexRule.RegexRule;
            Regex regex = new Regex(patten, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (regex.IsMatch(sql)) ViewBag.Result1 = String.Join(",", regex.Matches(sql));
            if (regex.IsMatch(sqlUpdate)) ViewBag.Result2 = String.Join(",", regex.Matches(sqlUpdate));
            if (regex.IsMatch(sql)) ViewBag.Result1 = String.Join(",", regex.Matches(sql));
            ViewBag.Select = regex.IsMatch(sql);
            ViewBag.Update = regex.IsMatch(sqlUpdate);
            return View();
        }

        public async Task<IActionResult> TestAsync()
        {
            var task = Task.Run(() => {
                ViewBag.Thread2 = _first.GetStringAsync("I'm Second").Result;
            });
            ViewBag.Thread1 = await _first.GetStringAsync("I'm First");
            ViewBag.Thread3 = _first.GetStringAsync("I'm Third").Result;
            ViewBag.ThreadCurrent = $@"Thread ID : {Thread.CurrentThread.ManagedThreadId}";
            return View();
        }

        public async Task<IActionResult> TestException()
        {
            throw new Exception();
        }

        [HttpGet]
        public IActionResult SetRedis()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetRedis(string key, string value)
        {
            if (String.IsNullOrEmpty(key)) _cache.SetValue(key, value);
            return RedirectToAction("GetRedis");
        }

        [HttpGet]
        public IActionResult GetRedis()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetRedis(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                var value = _cache.GetValue(key);
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