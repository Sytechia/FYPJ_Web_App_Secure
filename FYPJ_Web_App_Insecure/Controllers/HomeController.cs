using FYPJ_Web_App_Insecure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Sqlite;
using System.Net;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace FYPJ_Web_App_Insecure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly databaseContext _dbc;
        public HomeController(databaseContext dbc, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbc = dbc;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("The privacy page has been accessed");
            return View();
        }
        public IActionResult About()
        {
            _logger.LogInformation("The about page has been accessed");
            return View();
        }
        public IActionResult AboutFixed()
        {
            
            return View();
        }

        [HttpGet("/Home/Search")]
        public IActionResult Search()
        {
            
            _logger.LogInformation("The search page has been accessed");
            HttpContext.Response.Cookies.Append("XSS_Challenge_3", $"Successful");


            return View();
        }

        public IActionResult Test()
        {

            return View();
        }




    }
}



