using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class BookingAdditionalConfiguration : IEntityTypeConfiguration<BookingAdditionalServicesEntity>
    {
        public void Configure(EntityTypeBuilder<BookingAdditionalServicesEntity> builder)
        {
            builder
                .HasKey(bs => new { bs.BookingID, bs.AdditionalServicesID });

            builder
                .HasOne(a => a.BookingEntity)
                .WithMany(a => a.BookingsServces)
                .HasForeignKey(a => a.BookingID);

            builder
                .HasOne(a => a.AdditionalServicesEntity)
                .WithMany(a => a.BookingsServces)
                .HasForeignKey(a => a.AdditionalServicesID);
        }
    }
}
