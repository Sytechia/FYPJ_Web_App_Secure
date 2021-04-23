using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FYPJ_Web_App_Insecure.Models
{
    public class databaseContext : DbContext
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public databaseContext(DbContextOptions<databaseContext> options, IWebHostEnvironment webHostEnvironment)
            : base(options)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public static readonly Microsoft.Extensions.Logging.LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
        new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
        });


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_webHostEnvironment.ContentRootPath}/database.db");
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        
       
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }

        
        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<BlogResponse> BlogResponses { get; set; }


    }
}
