using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class AgeGroupConfiguration : IEntityTypeConfiguration<AgeGroup>
{
    public void Configure(EntityTypeBuilder<AgeGroup> builder)
    {
        builder.ToTable("AgeGroups");

        builder.HasKey(ag => ag.Id);

        builder.Property(ag => ag.Id).ValueGeneratedOnAdd();

        builder.Property(ag => ag.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ag => ag.Weights)
            .HasConversion(new AgeGroupWeightsConvertor())
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(ag => ag.Styles)
            .HasConversion(new AgeGroupStylesConvertor())
            .IsRequired()
            .HasColumnType("jsonb");

        builder.HasOne(ag => ag.Competition)
            .WithMany(cd => cd.AgeGroups)
            .HasForeignKey(ag => ag.CompetitionDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
