using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYPJ_Web_App_Insecure.Models
{
    public class Products
    {
        [Key]
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Product_Quantity { get; set; }
        public string image { get; set; }






    }

    //public class Genres
    //{
    //    [Key]
    //    public string GenreID { get; set; }
    //    public string genreName { get; set; }

    //    public string Genre { get; set; }

    //    public Products Products { get; set; }
    //}
}
