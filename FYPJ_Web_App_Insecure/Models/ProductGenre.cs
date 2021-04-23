using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace FYPJ_Web_App_Insecure.Models
{
    public class ProductGenre
    {
        public List<Products> Product { get; set; }
        public SelectList Genres { get; set; }
        public string ProductGenres { get; set; }
        public string SearchString { get; set; }
    }
}
