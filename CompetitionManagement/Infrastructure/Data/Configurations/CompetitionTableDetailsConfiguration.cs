using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class CompetitionTableDetailsConfiguration : IEntityTypeConfiguration<CompetitionTableDetail>
{
    public void Configure(EntityTypeBuilder<CompetitionTableDetail> builder)
    {
        builder.ToTable("CompetitionTableDetails");

        builder.HasKey(ctd => ctd.Id);
        
        builder.Property(ctd => ctd.Id).ValueGeneratedOnAdd();

        builder.Property(ctd => ctd.CompetitionTableId)
            .IsRequired();

        builder.Property(ctd => ctd.FirstCompetitionRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.SecondRedCompetitionRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.Status)
            .IsRequired();
          
        builder.HasOne(x => x.Competition)
            .WithMany(x => x.CompetitionTableDetails)
            .HasForeignKey(x => x.CompetitionTableId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.FirstCompetitionRegister)
            .WithMany()
            .HasForeignKey(x => x.FirstCompetitionRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.SecondRedCompetitionRegister)
            .WithMany()
            .HasForeignKey(x => x.SecondRedCompetitionRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
