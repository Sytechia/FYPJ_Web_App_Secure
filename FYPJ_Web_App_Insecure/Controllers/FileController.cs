using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using FYPJ_Web_App_Insecure.Models;
namespace FYPJ_Web_App_Insecure.Controllers
{
    public class FileController : Controller
    {

        private IHostingEnvironment Environment;
        [Obsolete]
        public FileController(IHostingEnvironment _environment)
    {
        Environment = _environment;
    }
 
    public IActionResult Index()
    {
        //Fetch all files in the Folder (Directory).
        string[] filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "Files/"));
 
        //Copy File names to Model collection.
        List<FileModel> files = new List<FileModel>();
        foreach (string filePath in filePaths)
        {
            files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
        }
 
        return View(files);
    }
 
    public FileResult DownloadFile(string fileName)
    {
        //Build the File Path.
        string path = Path.Combine(this.Environment.WebRootPath, "Files/") + fileName;
 
        //Read the File data into Byte Array.
        byte[] bytes = System.IO.File.ReadAllBytes(path);
 
        //Send the File to Download.
        return File(bytes, "application/octet-stream", fileName);
    }

    }
}
