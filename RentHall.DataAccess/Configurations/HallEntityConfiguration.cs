using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class HallEntityConfiguration : IEntityTypeConfiguration<HallEntity>
    {
        public void Configure(EntityTypeBuilder<HallEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(a => a.Bookings)
                .WithOne(a => a.HallEntity)
                .HasForeignKey(a => a.HallID);

            builder
                .HasMany(b => b.HallsServices)
                .WithOne(b => b.HallEntity)
                .HasForeignKey(b => b.HallID);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.Property(d => d.Сapacity)
                .IsRequired();

        }
    }
}
