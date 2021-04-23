using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Diagnostics;

namespace FYPJ_Web_App_Insecure.Controllers
{
    public class XMLController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public XMLController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]

        public IActionResult Index()
        {
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            Process[] localByName = Process.GetProcessesByName("Vmmem");
            long totalBytesOfMemoryUsed = 0;
            long totalBytesOfMemoryUsedVS = ((currentProcess.WorkingSet64) / 1024 / 1024);
            foreach (var x in localByName)
            {
                totalBytesOfMemoryUsed += ((x.WorkingSet64) / 1024 / 1024);
                if (totalBytesOfMemoryUsed >= 4000)
                {
                    HttpContext.Response.Cookies.Append("XXE_Challenge_1", $"Successful");
                }
            }
            if (totalBytesOfMemoryUsedVS >= 1000)
            {
                HttpContext.Response.Cookies.Append("XXE_Challenge_1", $"Successful");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(string content)
        {
            try
            {
                // Gets the data from the Ajax POST
                string firstname = Request.Form["xml"];
                // sets the location of of project directory
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string path = "";
                //Path to save the file
                path = Path.Combine(contentRootPath, "WriteText.xml");
                //Write to file
                await ExampleAsync(firstname);
                //XML Parsing settings
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.XmlResolver = new XmlUrlResolver();
                settings.MaxCharactersFromEntities = 0;
                settings.Async = true;
                //Initiate XML parsing
                XmlReader reader = XmlReader.Create(path, settings);
                while (reader.Read())
                {
                    Console.WriteLine(reader.Value);
                    if (reader.Value.Contains("C:/Windows/system.ini") || reader.Value.Contains("etc/passwd"))
                    {
                        HttpContext.Response.Cookies.Append("XXE_Challenge_2", $"Successful");
                    }
                }
                reader.Close();
                ViewBag.Successful = "Successful!";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Something went wrong please try again";
                return View();
            }
        }

        public static async Task ExampleAsync(string firstname)
        {
            string text = firstname;
            await System.IO.File.WriteAllTextAsync("WriteText.xml", text);
        }

    }
}
