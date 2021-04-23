using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FYPJ_Web_App_Insecure.Models
{
    public class CartUser
    {
        public Cart Cart { get; set; }
        public List<CartItem> CartItem { get; set; }
        public List<Products> Products { get; set; }
        public Int16 SubTotal { get; set; }
    }
}
