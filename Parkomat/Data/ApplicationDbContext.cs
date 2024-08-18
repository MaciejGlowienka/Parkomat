using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parkomat.Models;
using System.Security.Claims;

namespace Parkomat.Data
{

    /// <summary>
    /// The database context for the application, inheriting from IdentityDbContext to include identity features.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// Gets or sets the DbSet for Parkings.
        /// </summary>
        public DbSet<Parking> Parkings { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for ParkingLots.
        /// </summary>
        public DbSet<ParkingLot> ParkingsLots { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for PriceLists.
        /// </summary>
        public DbSet<PriceList> PriceLists { get; set; }

        /// <summary>
        /// Gets or sets the DbSet for ApplicationUsers.
        /// </summary>
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// Configures the schema needed for the identity framework and seeds initial data.
        /// </summary>
        /// <param name="modelBuilder">The builder used to construct the model for the context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriceList>().HasData(
               new PriceList
               {
                   PriceListId = 1,
                   Hour1 = 4.50m,
                   Hour2 = 5.40m,
                   Hour3 = 6.40m,
                   Rest = 4.50m
               });


            modelBuilder.Entity<ParkingLot>().HasData(
                new ParkingLot
                {
                    ParkingLotId = 1,
                    ParkingLotName = "Strefa Płatnego Parkowania Niestrzeżonego w Warszawie",
                    PriceListId = 1
                });


            modelBuilder.Entity<Parking>()
                .HasOne(p => p.User)
                .WithMany(u => u.Parkings)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }
    }
}
