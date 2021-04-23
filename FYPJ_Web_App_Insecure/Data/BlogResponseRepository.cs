using FYPJ_Web_App_Insecure.Models;
using System;

namespace FYPJ_Web_App_Insecure.Data
{
    public class BlogResponseRepository
    {
        private readonly databaseContext _context;

        public BlogResponseRepository(databaseContext context)
        {
            _context = context;
        }

        public void CreateBlogResponse(BlogResponse response)
        {
            try 
            {
                
                _context.BlogResponses.Add(response);
                _context.SaveChanges();
            }

            catch (Exception)
            {
 
            }
           
        }
    }
}
