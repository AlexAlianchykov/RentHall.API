using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class AdditionalServicesEntityConfiguration : IEntityTypeConfiguration<AdditionalServicesEntity>
    {
        public void Configure(EntityTypeBuilder<AdditionalServicesEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(a => a.HallsServices)
                .WithOne(a => a.AdditionalServicesEntity)
                .HasForeignKey(a => a.AdditionalServicesID);

            builder
                .HasMany(a => a.BookingsServces)
                .WithOne(a => a.AdditionalServicesEntity)
                .HasForeignKey(a => a.AdditionalServicesID);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
