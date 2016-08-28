using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempWebAppMVC.Models
{
    public class ShippingDetailsViewModel
    {
        public decimal TotalPrice { get; set; }
        public User User { get; set; }
        //public Address Address { get; set; }
    }
}