using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FYPJ_Web_App_Insecure.Controllers
{
    public class ChallengeTrackingController : Controller
    {
        public IActionResult Index()
        {
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = (currentProcess.WorkingSet64) / 1024 / 1024;
            if (totalBytesOfMemoryUsed >= 1000)
            {
                HttpContext.Response.Cookies.Append("XXE_Challenge_1", $"Successful");
            }
            if (HttpContext.Request.Cookies["SQL_Injection_Challenge_1"] == "Successful")
            {
                ViewBag.SQL_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["SQL_Injection_Challenge_2"] == "Successful")
            {
                ViewBag.SQL_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["SQL_Injection_Challenge_3"] == "Successful")
            {
                ViewBag.SQL_Challenge3 = "Successful";
            }
            if (HttpContext.Request.Cookies["Broken_Access_Challenge_1"] == "Successful")
            {
                ViewBag.BrokenAccess_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["Broken_Access_Challenge_2"] == "Successful")
            {
                ViewBag.BrokenAccess_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["XXE_Challenge_1"] == "Successful")
            {
                ViewBag.XXE_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["XXE_Challenge_2"] == "Successful")
            {
                ViewBag.XXE_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["Broken_Authentication_Challenge_1"] == "Successful")
            {
                ViewBag.Broken_Authentication_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["Broken_Authentication_Challenge_2"] == "Successful")
            {
                ViewBag.Broken_Authentication_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["Sensitive_Data_Challenge_1"] == "Successful")
            {
                ViewBag.Sensitive_Data_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["Sensitive_Data_Challenge_2"] == "Successful")
            {
                ViewBag.Sensitive_Data_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["Sensitive_Data_Challenge_3"] == "Successful")
            {
                ViewBag.Sensitive_Data_Challenge3 = "Successful";
            }
            if (HttpContext.Request.Cookies["Sensitive_Data_Challenge_4"] == "Successful")
            {
                ViewBag.Sensitive_Data_Challenge4 = "Successful";
            }
            if (HttpContext.Request.Cookies["Security_Misconfig_Challenge_1"] == "Successful")
            {
                ViewBag.Misconfig_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["Security_Misconfig_Challenge_2"] == "Successful")
            {
                ViewBag.Misconfig_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["Security_Misconfig_Challenge_3"] == "Successful")
            {
                ViewBag.Misconfig_Challenge3 = "Successful";
            }
            if (HttpContext.Request.Cookies["Security_Misconfig_Challenge_4"] == "Successful")
            {
                ViewBag.Misconfig_Challenge4 = "Successful";
            }
            if (HttpContext.Request.Cookies["XSS_Challenge_1"] == "Successful")
            {
                ViewBag.XSS_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["XSS_Challenge_2"] == "Successful")
            {
                ViewBag.XSS_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["XSS_Challenge_3"] == "Successful")
            {
                ViewBag.XSS_Challenge3 = "Successful";
            }
            if (HttpContext.Request.Cookies["XSS_Challenge_4"] == "Successful")
            {
                ViewBag.XSS_Challenge4 = "Successful";
            }
            if (HttpContext.Request.Cookies["Vulnerabilities_Challenge_1"] == "Successful")
            {
                ViewBag.Vulnerabilities_Challenge1 = "Successful";
            }
            if (HttpContext.Request.Cookies["Vulnerabilities_Challenge_2"] == "Successful")
            {
                ViewBag.Vulnerabilities_Challenge2 = "Successful";
            }
            if (HttpContext.Request.Cookies["Logging_Challenge_1"] == "Successful")
            {
                ViewBag.Logging_Challenge1 = "Successful";
            }
            return View();


        }
    }
}
