using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class CompetitionRegisterConfiguration : IEntityTypeConfiguration<CompetitionRegister>
{
    public void Configure(EntityTypeBuilder<CompetitionRegister> builder)
    {
        builder.ToTable("CompetitionRegisters");

        builder.HasKey(cr => cr.Id);
        
        builder.Property(cr => cr.Id).ValueGeneratedOnAdd();

        builder.Property(cr => cr.CoachPhoneNumber)
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(cr => cr.Status)
            .IsRequired();

        builder.Property(cr => cr.Weight)
            .IsRequired();

        builder.Property(cr => cr.Style)
            .IsRequired();

        builder.HasOne(x => x.AthleteUser)
            .WithMany()
            .HasForeignKey(x => x.AthleteUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
            
        builder.HasOne(x => x.CoachUser)
            .WithMany()
            .HasForeignKey(x => x.CoachUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        builder.HasOne(x => x.AgeGroup)
            .WithMany()
            .HasForeignKey(x => x.AgeGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
