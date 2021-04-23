using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using FYPJ_Web_App_Insecure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;    
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace FYPJ_Web_App_Insecure.Controllers
{
    public class AdminController : Controller
    {
        // GET: AdminController
        private readonly databaseContext _db;
        public AdminController(databaseContext db)
        {
            _db = db;
        }
        [HttpGet("/admin")]
        public IActionResult Admin()
        {
            HttpContext.Response.Cookies.Append("Sensitive_Data_Challenge_3", $"Successful");
            if (HttpContext.Request.Cookies["user_id"] == "1")
            {
                List<Customer> customers = (from customer in _db.Customer.Take(1000)
                                            select customer).ToList();
                return View(customers);
            }
            else
            {
                ViewBag.NotAllowed = "You do not have permission to view this page";
                return View();
            }
        }


    }

}




