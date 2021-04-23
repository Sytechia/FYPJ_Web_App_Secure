using FYPJ_Web_App_Insecure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FYPJ_Web_App_Insecure.Data
{
    public class BlogEntryRepository
    {
        private readonly databaseContext _context;

        public BlogEntryRepository(databaseContext context)
        {
            _context = context;
        }

        public BlogEntry CreateBlogEntry(string title, string contents) //string username
        {
            var entry = new BlogEntry
            {
                Title = title,
                Contents = contents,
                //Author = username,
                //PostedDate = DateTime.Now,
            };

            entry = _context.BlogEntries.Add(entry).Entity;
            _context.SaveChanges();
            return entry;
        }

        public BlogEntry GetBlogEntry(int blogEntryId)
        {
            return _context.BlogEntries.Single(b => b.Id == blogEntryId);
        }

        public List<BlogEntry> GetTopBlogEntries()
        {
            return GetTopBlogEntries(4, 0);
        }

        public List<BlogEntry> GetTopBlogEntries(int numberOfEntries, int startPosition)
        {
            var blogEntries = _context.BlogEntries
                .OrderByDescending(b => b.PostedDate)
                .Skip(startPosition)
                .Take(numberOfEntries);
            return blogEntries.ToList();
        }
    }
}
