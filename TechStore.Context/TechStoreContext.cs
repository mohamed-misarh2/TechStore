using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Models;

namespace TechStore.Context
{
    public class TechStoreContext : IdentityDbContext<TechUser>
    {
        public TechStoreContext(DbContextOptions<TechStoreContext> options)
            : base(options)
        {
        }
    }
}
