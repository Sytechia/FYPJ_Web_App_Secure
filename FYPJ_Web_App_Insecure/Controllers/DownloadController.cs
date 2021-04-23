using FYPJ_Web_App_Insecure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FYPJ_Web_App_Insecure.Controllers
{


    public class DownloadController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly IActionDescriptorCollectionProvider provider;
        public DownloadController(IConfiguration _configuration, IActionDescriptorCollectionProvider provider)
        {
            Configuration = _configuration;
            this.provider = provider;
        }
        [HttpGet("/Routes")]
        public IActionResult Routes()
        {

            var urls = this.provider.ActionDescriptors.Items
        .Select(descriptor => '/' + string.Join('/', descriptor.RouteValues.Values
                                                                        .Where(v => v != null)
                                                                        .Select(c => c.ToLower())
                                                                        .Reverse()))
        .Distinct()
        .ToList();

            HttpContext.Response.Cookies.Append("Sensitive_Data_Challenge_2", $"Successful");
            return Ok(urls);

        }
        [HttpGet("/Download_database")]
        public async Task<IActionResult> Download()
        {
            try
            {
                var contentRoot = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
                var path = contentRoot + @"\database.db";
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                var ext = Path.GetExtension(path).ToLowerInvariant();
                HttpContext.Response.Cookies.Append("Sensitive_Data_Challenge_1", $"Successful");
                return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));
            }
            catch
            {
                ViewBag.Error("Something went wrong");
                return ViewBag.Error;
            }
        }
        [HttpGet("/Logs")]
        public IActionResult Logs()
        {
            HttpContext.Response.Cookies.Append("Sensitive_Data_Challenge_4", $"Successful");

            var contentRoot = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            string[] filePaths = Directory.GetFiles(Path.Combine(contentRoot, "Logs/"));

            //Copy File names to Model collection.
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }
            HttpContext.Response.Cookies.Append("Logging_Challenge_1", $"Successful");
            return View(files);
        }
        public FileResult DownloadFile(string fileName)
        {
            var contentRoot = Configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            //Build the File Path.
            string path = Path.Combine(contentRoot, "Logs/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".jpg","image/jpeg"},
                { ".db","application/x-sqlite3"}
            };
        }
    }
}

