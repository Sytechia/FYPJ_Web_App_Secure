using FYPJ_Web_App_Insecure.Models;
using FYPJ_Web_App_Insecure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FYPJ_Web_App_Insecure.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        private readonly BlogEntryRepository _blogEntryRepository;
        private readonly BlogResponseRepository _blogResponseRepository;
        private readonly databaseContext _context;
        private readonly ILogger<HomeController> _logger;

        public BlogController(BlogEntryRepository blogEntryRepository, BlogResponseRepository blogResponseRepository, databaseContext context, ILogger<HomeController> logger)
        {
            _blogEntryRepository = blogEntryRepository;
            _blogResponseRepository = blogResponseRepository;
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Blog has been accessed");
            var blogE = _blogEntryRepository.GetTopBlogEntries();
            for (var i = 0; i < blogE.Count; i++)
            {
                var blogERes = _context.BlogResponses.Where(x => x.BlogEntryId.Equals(blogE[i].Id)).ToList();
                blogE[i].Responses = blogERes;

            }
            return View(blogE);
        }

        [HttpGet("{entryId}")]
        public IActionResult Reply(int entryId)
        {
            HttpContext.Response.Cookies.Append("XSS_Challenge_1", $"Successful");
            return View(_blogEntryRepository.GetBlogEntry(entryId));
        }

        [HttpPost("{entryId}")]
        public IActionResult Reply(int entryId, string contents)
        {
            var userName = User?.Identity?.Name ?? "Anonymous";
            var response = new BlogResponse()
            {
                //Author = Username,
                Contents = contents,
                BlogEntryId = entryId,
                ResponseDate = DateTime.Now
            };
            _blogResponseRepository.CreateBlogResponse(response);
           

            return RedirectToAction("Index");
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult Create(string title, string contents)
        {
            var blogEntry = _blogEntryRepository.CreateBlogEntry(title, contents);
            return View(blogEntry);
        }

    }
}