﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class TechUser :IdentityUser
    {
      
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string? Address { get; set; }
        public string? image {  get; set; }
        public string? AccountStatus { get; set; }
        public string? PaymentInformation { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
       

        public TechUser() 
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }
    }
}
