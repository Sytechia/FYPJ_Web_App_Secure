using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FYPJ_Web_App_Insecure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace FYPJ_Web_App_Insecure.Controllers
{
    [AutoValidateAntiforgeryToken]

    public class AccountController : Controller
    {
        private readonly databaseContext _dbc;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;

        public AccountController(databaseContext dbc, IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger)
        {
            _dbc = dbc;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
      
        public IActionResult Index()
        {
            _logger.LogInformation("The register page has been accessed");

            return View();
        }

        static string HashSh1(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hashSh1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

                // declare stringbuilder
                var sb = new StringBuilder(hashSh1.Length * 2);

                // computing hashSh1
                foreach (byte b in hashSh1)
                {
                    // "x2"
                    sb.Append(b.ToString("X2").ToLower());
                }

                // final output
                return sb.ToString();
            }
        }

        static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
        static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }
        static void EncryptAesManaged(string raw)
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    Console.WriteLine(aes.Key);
                    Console.WriteLine(aes.IV);
                    Console.WriteLine(Encoding.Default.GetString(aes.Key));
                    Console.WriteLine(Encoding.Default.GetString(aes.IV));
                    // Encrypt string    
                    byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    // Print encrypted string    
                    Console.WriteLine($"Encrypted data: {System.Text.Encoding.UTF8.GetString(encrypted)}");
                    // Decrypt the bytes to a string.    
                    string decrypted = Decrypt(encrypted, aes.Key, aes.IV);
                    // Print decrypted string. It should be same as raw data    
                    Console.WriteLine($"Decrypted data: {decrypted}");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(Customer cc)
        {

            var usr = _dbc.Customer.Where(x => x.Username.Equals(cc.Username));
            var email = _dbc.Customer.Where(x => x.Uemail.Equals(cc.Uemail));
            if (usr.FirstOrDefault() != null)
            {
                ViewBag.message = "The username " + cc.Username + " already exists!";
            }
            if (email.FirstOrDefault() != null)
            {
                ViewBag.message = "The email " + cc.Uemail + " already exists!";
            }
            else
            {
                cc.Active = "1";
                cc.Pwd = HashSh1(cc.Pwd);
                _dbc.Add(cc);
                _dbc.SaveChanges();
                ViewBag.message = "The User " + cc.Username + " Is Saved Successfully!";
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("The login page has been accessed");
            ViewBag.Notification = TempData["ErrorMessage"];
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Login(Customer model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                EncryptAesManaged("HELLO");
                if (model.Pwd != null && model.Username != null)
                {

                    var usr = _dbc.Customer.Where(x => x.Username.Equals(model.Username) && x.Pwd.Equals(HashSh1(model.Pwd)));
                    if (usr.FirstOrDefault() == null)
                    {
                        ViewBag.Notification = "You have entered a wrong password or username";
                    }
                    else if (usr.FirstOrDefault() != null)
                    {
                        HttpContext.Response.Cookies.Append("user_id", HashSh1($"{usr.FirstOrDefault().UserId}"));
                        HttpContext.Session.SetString("UserId", HashSh1($"{usr.FirstOrDefault().UserId}"));
                        HttpContext.Session.SetString("Username", $"{usr.FirstOrDefault().Username}");
                        return Redirect("/Home");
                    }
                }
                else
                {
                    ViewBag.Notification = "Please fill in your credentials!";
                }
            }
            catch (Exception)
            {
                ViewBag.Notification = "Something went wrong, please try again";
            }

            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete("user_id");
            }
            return Redirect("/Account/Login");
        }

        // Used for secure site
        //public IActionResult myAccount()
        //{
        //var uId = HttpContext.Session.GetString("UserId");

        //var user = _dbc.Customer.Where(x => x.UserId.Equals(uId)).FirstOrDefault();
        //System.Diagnostics.Debug.WriteLine("uId: ", uId);
        //System.Diagnostics.Debug.WriteLine("User: ", user);

        //return View(user);
        //}

        [HttpGet("Account/Deactivate")]
        [HttpGet("Account/Deactivate/{id:int}")]
        public IActionResult Deactivate(int id)
        {
            _logger.LogInformation("The deactivate page has been accessed");
            var user = _dbc.Customer.Where(x => x.UserId.Equals(id)).FirstOrDefault();
            user.Active = "0";
            _dbc.SaveChanges();
            return Redirect("/Account/Login");
        }

        [HttpGet("Account/myAccount")]
        [HttpGet("Account/myAccount/{id:int}")]
        public IActionResult myAccount(int id)
        {
            _logger.LogInformation("The account page has been accessed");

            var user = _dbc.Customer.Where(x => x.UserId.Equals(id)).FirstOrDefault();
            if (HttpContext.Request.Cookies["user_id"] == "1")
            {
                ViewBag.isAdmin = "True";
            }

            var orderHist = _dbc.OrderDetails.Where(x => x.CustomerID.Equals(id)).ToList();

            var userT = new CustomerTrans();
            userT.UserId = user.UserId;
            userT.Username = user.Username;
            userT.Pwd = user.Pwd;
            userT.Confirmpwd = user.Confirmpwd;
            userT.Uemail = user.Uemail;
            userT.Postal_code = user.Postal_code;
            userT.City = user.City;
            userT.Phone = user.Phone;
            userT.Active = user.Active;
            userT.Secret_Ans = user.Secret_Ans;
            userT.Secret_Qns = user.Secret_Qns;

            userT.orderHistory = orderHist;

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            Console.WriteLine(_webHostEnvironment.ContentRootPath);
            string[] fileArray = Directory.GetFiles(_webHostEnvironment.ContentRootPath);
            foreach (var x in fileArray)
            {
                Console.WriteLine(x);
            }
            connectionStringBuilder.DataSource = _webHostEnvironment.ContentRootPath + @"/database.db";
            Console.Write(connectionStringBuilder.ConnectionString);
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "select * from Products";
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
            /*
            List<Products> newModel = new List<Products>();
            for (var i = 0; i < orderHist.Count; i ++)
            {
                for (var j = 0; j < model.Count; j++)
                {
                    if (orderHist[i].ProductId == model[j].ProductId)
                    {

                    }

                }

            } */
            userT.products = model;

            return View(userT);
        }

        private string GetImageUrlForProduct(string id)
        {
            var imageUrl = $"/files/Images/ProductImages/{id}.jpg";
            var path = _webHostEnvironment.WebRootFileProvider.GetFileInfo(imageUrl);
            return imageUrl;
        }

        [HttpGet("Account/editAccount")]
        [HttpGet("Account/editAccount/{id:int}")]
        public IActionResult EditAccount(int id)
        {
            _logger.LogInformation("The edit account page has been accessed");

            var user = _dbc.Customer.Where(x => x.UserId.Equals(id)).FirstOrDefault();
            HttpContext.Response.Cookies.Append("XSS_Challenge_2", $"Successful");

            return View(user);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult EditAccount(Customer model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _dbc.Customer.Where(x => x.UserId.Equals(model.Username)).FirstOrDefault();
            user.Username = model.Username;
            user.Pwd = HashSh1(model.Pwd);
            user.Confirmpwd = HashSh1(model.Confirmpwd);
            _dbc.SaveChanges();
            return Redirect("/Account/myAccount/" + user.UserId);
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(Customer model)
        {
            try
            {
                if (model.Pwd != model.Confirmpwd)
                {
                    ViewBag.NotSamePassword = "The passwords are not the same please try again!";
                }
                else
                {
                    var user = _dbc.Customer.Where(x => x.Uemail.Equals(model.Uemail));
                    if (user.FirstOrDefault() != null)
                    {
                        if (model.Secret_Qns != user.FirstOrDefault().Secret_Qns)
                        {
                            ViewBag.WrongQuestion = "You secret question is wrong!";
                        }
                        else if (model.Secret_Ans != user.FirstOrDefault().Secret_Ans)
                        {
                            ViewBag.WrongAnswer = "You secret answer is wrong!";
                        }
                        else
                        {
                            user.FirstOrDefault().Pwd = HashSh1(model.Pwd);
                            user.FirstOrDefault().Confirmpwd = HashSh1(model.Confirmpwd);
                            _dbc.SaveChanges();
                            ViewBag.Successfull = "Changed Sucessfully";
                        }
                    }
                    else
                    {
                        ViewBag.NotValidUser = "The user does not exist!";
                    }

                }
            }
            catch (Exception)
            {
                ViewBag.Notification = "Something went wrong please try again";
            }

            return View();
        }




    }
}
