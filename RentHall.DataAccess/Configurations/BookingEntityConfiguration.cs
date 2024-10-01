using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class BookingEntityConfiguration : IEntityTypeConfiguration<BookingEntity>
    {
        public void Configure(EntityTypeBuilder<BookingEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(a => a.UserEntity)
                .WithMany(a => a.Bookings)
                .HasForeignKey(a => a.UserID);

            builder
                .HasOne(b => b.HallEntity)
                .WithMany(b => b.Bookings)
                .HasForeignKey(b => b.HallID);

            builder
                .HasMany(c => c.BookingsServces)
                .WithOne(c => c.BookingEntity)
                .HasForeignKey(c => c.BookingID);
        }
    }
}
