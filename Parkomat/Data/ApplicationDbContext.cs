using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parkomat.Models;
using System.Security.Claims;

namespace Parkomat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Parking> Parkings { get; set; }
        public DbSet<ParkingLot> ParkingsLots { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        

            base.OnModelCreating(modelBuilder);


            
            modelBuilder.Entity<Parking>()
                .HasOne(p => p.User)
                .WithMany(u => u.Parkings)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }
    }
}
