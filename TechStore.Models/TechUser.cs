using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Models
{
    public class TechUser :IdentityUser
    {
      
        public string? Address { get; set; }
        public string? AccountStatus { get; set; }
        public string? PaymentInformation { get; set; }

        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
