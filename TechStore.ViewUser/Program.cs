using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TechStore.Application.Contract;
using TechStore.Application.Services;
using TechStore.Context;
using TechStore.Dtos.UserDTO;
using TechStore.Infrastructure;
using TechStore.Models;

namespace TechStore.ViewUser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<TechStoreContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));
            builder.Services.AddIdentity<TechUser, IdentityRole>().AddEntityFrameworkStores<TechStoreContext>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);


            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategorySpecificationsRepository, CategorySpecificationsRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IproductCategorySpecifications,ProductCategorySpecificationsRepository>();
            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IReviewService, ReviewServices>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<IspecificationsRepository, SpecificationsRepository>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "TechStore.Session";
                options.IdleTimeout = TimeSpan.MaxValue;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

                                    



       
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var session = context.Session;
                var lastAccessed = session.GetString("LastAccessed");
                var cartLastAccessed = session.GetString("CartLastAccessed");

                if (!string.IsNullOrEmpty(lastAccessed) && DateTime.TryParse(lastAccessed, out DateTime lastAccessedTime))
                {
                    var expirationTime = lastAccessedTime.AddMinutes(30); // Adjust as needed
                    if (DateTime.Now > expirationTime)
                    {
                        // Session has expired, clear session data
                        session.Clear();
                        context.Response.Redirect("/sessionExpired"); // Redirect to a session expired page
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(cartLastAccessed) && DateTime.TryParse(cartLastAccessed, out DateTime cartLastAccessedTime))
                {
                    var cartExpirationTime = cartLastAccessedTime.AddMinutes(30); // Adjust as needed
                    if (DateTime.Now > cartExpirationTime)
                    {
                        // Cart data has expired, clear cart data
                        session.Remove("Cart");
                        session.Remove("CartLastAccessed");
                    }
                }

                session.SetString("LastAccessed", DateTime.Now.ToString()); // Update last accessed time
                await next();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}