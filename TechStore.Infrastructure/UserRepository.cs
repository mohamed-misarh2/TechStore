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
    public class UserRepository : Repository<TechUser,int>, IUserRepository
    {
        private readonly TechStoreContext _context;
       
        public UserRepository(TechStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TechUser> SearchUserByName(string name)
        {
            return await _context.Users.FindAsync(name);
        }

      
    }
}
