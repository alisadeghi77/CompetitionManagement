using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(u => u.NationalId)
            .IsRequired(false)
            .HasMaxLength(10);

        builder.Property(u => u.BirthDate)
            .IsRequired(false);
        
        builder.HasMany(c => c.Roles)
            .WithMany();
    }
}
