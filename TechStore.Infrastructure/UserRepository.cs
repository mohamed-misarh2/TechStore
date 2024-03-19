using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Application.Contract;
using TechStore.Context;
using TechStore.Models;

namespace TechStore.Infrastructure
{
    public class UserRepository : Repository<TechUser,string>, IUserRepository
    {
        private readonly TechStoreContext _context;
       
        public UserRepository(TechStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TechUser>> SearchUserByName(string name)
        {
            var data= await _context.Users.Where(u=>u.FirstName ==name|| u.LastName==name).Select(u=>u).ToListAsync();
            return data;
        }

      
    }
}
