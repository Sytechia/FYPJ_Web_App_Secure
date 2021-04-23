using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using FYPJ_Web_App_Insecure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FYPJ_Web_App_Insecure.Controllers
{

    [AutoValidateAntiforgeryToken]
    public class ShopController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly databaseContext _db;
        private readonly ILogger<HomeController> _logger;
        private Products GetProductById(string id)
        {
            var product = _db.Products.FromSqlRaw($"SELECT * FROM products WHERE ProductId='{id}'");
            return product.FirstOrDefault();
        }

        public ShopController(IWebHostEnvironment webHostEnvironment, databaseContext db, ILogger<HomeController> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
            _logger = logger;
        }

        [IgnoreAntiforgeryToken]
        public IActionResult Index(string searchString)
        {
            _logger.LogInformation("The shop page has been accessed");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            Console.WriteLine(_webHostEnvironment.ContentRootPath);
            string[] fileArray = Directory.GetFiles(_webHostEnvironment.ContentRootPath);
            foreach(var x in fileArray)
            {
                Console.WriteLine(x);
            }
            connectionStringBuilder.DataSource = _webHostEnvironment.ContentRootPath + @"/database.db";
            Console.Write(connectionStringBuilder.ConnectionString);
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var selectCmd = connection.CreateCommand();
            //selectCmd.CommandText = "select * from products where title = '100' or '1' = '1'";
            selectCmd.CommandText = "select * from products";
            var reader = selectCmd.ExecuteReader();

            List<Products> model = new List<Products>();
            while (reader.Read())
            {
                var items = new Products();
                items.ProductId = reader.GetString(0);
                items.Title = reader.GetString(1);
                items.Author = reader.GetString(2);
                items.Genre = reader.GetString(3);
                items.Price = reader.GetDecimal(4);
                items.image = GetImageUrlForProduct(reader.GetString(0));
                model.Add(items);
            }
            reader.Close();

            if (!String.IsNullOrEmpty(HttpContext.Request.Query["searchString"]))
            {

                var selectCmd_2 = connection.CreateCommand();
                selectCmd_2.CommandText = $"select * from products where title like'%{searchString}%'";
                var reader_2 = selectCmd_2.ExecuteReader();
                List<Products> filteredmodel = new List<Products>();
                while (reader_2.Read())
                {
                    Console.WriteLine(reader_2.GetString(0));
                    var items = new Products();
                    items.Title = reader_2.GetString(1);
                    items.Author = reader_2.GetString(2);
                    items.Genre = reader_2.GetString(3);
                    items.Price = reader_2.GetDecimal(4);
                    items.image = GetImageUrlForProduct(reader_2.GetString(0));
                    filteredmodel.Add(items);
                }
                reader.Close();
                var filter = filteredmodel.Where(s => s.Title.Contains(searchString));
                HttpContext.Response.Cookies.Append("XSS_Challenge_4", $"Successful");
                return View(filter);

            }
           

            return View(model);
        }
        [HttpGet]
        [Route("Shop/Details/{productID:int}")]
        public IActionResult Details(int productID, short quantity = 1)
        {
            var model = new Products();
            try
            {
                var product = GetProductById($"{productID}");
                model.ProductId = product.ProductId;
                model.Title = product.Title;
                model.Price = product.Price;
                model.Genre = product.Genre;
                model.Product_Quantity = product.Product_Quantity;
                model.image = GetImageUrlForProduct($"{productID}");
            }
            catch (InvalidOperationException)
            {
                ViewBag.ErrorMessage = "Product not found.";
            }
            return View(model);

        }

        private string GetImageUrlForProduct(string id)
        {
            var imageUrl = $"/Images/ProductImages/{id}.jpg";
            var path = _webHostEnvironment.WebRootFileProvider.GetFileInfo(imageUrl);
            return imageUrl;
        }
    }
}

