using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository;
using MWBAYLY.Repository.IRepository;
using MWBAYLY.Utlity;
using Stripe;
using System.ComponentModel;

namespace MWBAYLY
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
          
            //I Must put my code in this Location Between (Services) and (Build ) 
            builder.Services.AddDbContext<ApplicationDbContext>(
            option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));//This IS Responsiable with Dependancy Injection 

            // builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //  .AddDefaultTokenProviders();

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    options.User.AllowedUserNameCharacters =
            //        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@ "; // note the space at the end

            //    options.User.RequireUniqueEmail = true;
            //});
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<ICategoryRepository,CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();//This make is New Object 
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true; // Require email confirmation
            })
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender, EmailSender>(); 
                        var app = builder.Build();
        
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // ??? ?? Identity
            app.UseAuthorization();

           

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(
            name: "details",
           pattern: "Details/{id}",
          defaults: new { controller = "Home", action = "Details" });
            app.MapControllerRoute(
           name: "createnew",
          pattern: "CreateNew",
         defaults: new { controller = "Category", action = "CreateNew" });

            app.Run();
        }
    }
}
