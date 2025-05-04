using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CompetitionDefinitionConfiguration : IEntityTypeConfiguration<Competition>
{
    public void Configure(EntityTypeBuilder<Competition> builder)
    {
        builder.ToTable("CompetitionDefinitions");

        builder.HasKey(c => c.Id);

        var jsonOption = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        builder.Property(c => c.RegisterParams)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOption),
                v => JsonSerializer.Deserialize<CompetitionParam>(v, jsonOption))
            .IsRequired(false);
        
        builder.Property(c => c.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(c => c.Date)
            .IsRequired();

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.BannerImageId)
            .IsRequired();

        builder.Property(c => c.LicenseImageId)
            .IsRequired();
        
        builder.HasOne(ag => ag.PlannerUser)
            .WithMany(u => u.CompetitionDefinitions)
            .HasForeignKey(ag => ag.PlannerUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
