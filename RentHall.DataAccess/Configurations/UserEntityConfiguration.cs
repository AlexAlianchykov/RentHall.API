using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentHall.DataAccess.Entities;

namespace RentHall.DataAccess.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasMany(a => a.Bookings)
                .WithOne(a => a.UserEntity)
                .HasForeignKey(a => a.UserID);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.HashPassword)
                .IsRequired();

            builder.Property(d => d.Role)
                .IsRequired();

            builder.HasIndex(e => e.EmailUser)
                .IsUnique();
                
        }
    }
}
