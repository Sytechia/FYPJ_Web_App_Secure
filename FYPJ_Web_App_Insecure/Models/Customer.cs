using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FYPJ_Web_App_Insecure.Models
{
    public class Customer
    {
        [Key]
        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Pwd { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Confirmpwd { get; set; }
        [Display(Name = "Email")]
        public string Uemail { get; set; }
        [Display(Name = "Postal Code")]
        public string Postal_code { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Active")]
        public string Active { get; set; }
        public string Secret_Ans { get; set; }
        public string Secret_Qns { get; set; }
        //public List<OrderDetails> orderHistory { get; set; }
        //public List<Products> products { get; set; }

    }
}
