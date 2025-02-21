using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class FilesConfiguration : IEntityTypeConfiguration<FileEntity>
{
    public void Configure(EntityTypeBuilder<FileEntity> builder)
    {
        builder.ToTable("Files");

        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedOnAdd();

        builder.Property(f => f.Base64Content)
            .IsRequired();

        builder.Property(f => f.FileName)
            .IsRequired()
            .HasMaxLength(500);
    }
}
