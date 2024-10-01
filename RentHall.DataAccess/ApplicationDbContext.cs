using Microsoft.EntityFrameworkCore;
using RentHall.DataAccess.Configurations;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<HallEntity> Halls { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<AdditionalServicesEntity> Additionals { get; set; } 
        public DbSet<HallAdditionalServicesEntity> HallsAdditionals { get; set; }
        public DbSet<BookingAdditionalServicesEntity> BookingsAdditionals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HallEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BookingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AdditionalServicesEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HallAdditionalConfiguration());
            modelBuilder.ApplyConfiguration(new BookingAdditionalConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
