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
    public class ErrorPageController : Controller
    {

        private readonly databaseContext _db;
        public ErrorPageController(databaseContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

                List<Customer> customers = (from customer in _db.Customer.Take(1000)
                                            select customer).ToList();
                return View(customers);
            }

            }
        }



    
