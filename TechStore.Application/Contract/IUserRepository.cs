using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Application.Contract
{
    public interface IUserRepository : IRepository<TechUser, int>
    {
        Task<TechUser> SearchByNameUser(string name);
    }
}
