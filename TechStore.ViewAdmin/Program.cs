
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechStore.Application.Contract;
using TechStore.Application.Services;
using TechStore.Context;
using TechStore.Infrastructure;
using TechStore.Models;

namespace TechStore.ViewAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddScoped<ICategoryService, CategoryService>();
            //builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductItemRepository, ProductItemRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductItemService, ProductItemService>();

            builder.Services.AddDbContext<TechStoreContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //***

            builder.Services.AddIdentity<TechUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<TechStoreContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
