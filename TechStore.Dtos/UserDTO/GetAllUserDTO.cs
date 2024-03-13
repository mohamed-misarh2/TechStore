using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Dtos.UserDTO
{
    public class GetAllUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? image { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
    }
}
