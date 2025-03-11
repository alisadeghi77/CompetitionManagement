using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class CompetitionTableConfiguration : IEntityTypeConfiguration<CompetitionDetails>
{
    public void Configure(EntityTypeBuilder<CompetitionDetails> builder)
    {
        builder.ToTable("CompetitionTables");

        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.Id).ValueGeneratedOnAdd();

        builder.Property(ct => ct.Weight)
            .IsRequired();

        builder.Property(ct => ct.Style)
            .IsRequired();
        
        builder.HasOne(x => x.Competition)
            .WithMany()
            .HasForeignKey(x => x.CompetitionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.AgeGroup)
            .WithMany()
            .HasForeignKey(x => x.AgeGroupId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
