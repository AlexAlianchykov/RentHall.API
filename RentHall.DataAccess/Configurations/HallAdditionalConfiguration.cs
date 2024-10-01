using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class HallAdditionalConfiguration : IEntityTypeConfiguration<HallAdditionalServicesEntity>
    {
        public void Configure(EntityTypeBuilder<HallAdditionalServicesEntity> builder)
        {
            builder
                .HasKey(bs => new { bs.HallID, bs.AdditionalServicesID });

            builder
                .HasOne(a => a.HallEntity)
                .WithMany(a => a.HallsServices)
                .HasForeignKey(a => a.HallID);

            builder
                .HasOne(a => a.AdditionalServicesEntity)
                .WithMany(a => a.HallsServices)
                .HasForeignKey(a => a.AdditionalServicesID);
        }
    }
}
