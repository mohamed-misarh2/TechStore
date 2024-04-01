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
       
        public UserRepository(TechStoreContext context) : base(context)
        {
           
        }

        public async Task<IQueryable<TechUser>> SearchUserByName(string name)
        {
            return await Task.FromResult(_entities.Where(u => u.FirstName.Contains(name) || u.LastName.Contains(name)));
        }

      
    }
}
