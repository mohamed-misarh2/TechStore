using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.AccountDtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage= "First Name is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password  is Required")]
        public string Password { get; set; }

        [EmailAddress , Required(ErrorMessage = "Email Name is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = " Name is Required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Image is Required")]

        public IFormFile Image { get; set; }
    }
}
