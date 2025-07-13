using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MWBAYLY.Models;
using MWBAYLY.ViewModel;

namespace MWBAYLY.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
       // using Soild
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Category> categories { get; set; }
        
        /// //////////////////////////////
        
        public DbSet<Product>  Products{ get; set; }
        public DbSet<Company> Campanies { get; set; }
        public ApplicationDbContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            
            base.OnConfiguring(optionsBuilder);

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build()
                .GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(builder);



        }
		

		public DbSet<MWBAYLY.ViewModel.ApplicationUserVM> ApplicationUserVM { get; set; } = default!;
        public DbSet<MWBAYLY.ViewModel.LoginVM> LoginVM { get; set; } = default!;
    }
}
