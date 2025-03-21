using CompetitionManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompetitionManagement.Infrastructure.Data.Configurations;

public class CompetitionTableDetailsConfiguration : IEntityTypeConfiguration<CompetitionTable>
{
    public void Configure(EntityTypeBuilder<CompetitionTable> builder)
    {
        builder.ToTable("CompetitionTableDetails");

        builder.HasKey(ctd => ctd.Id);
        
        builder.Property(ctd => ctd.Id).ValueGeneratedOnAdd();

        builder.Property(ctd => ctd.FirstCompetitionRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.SecondCompetitionRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.Status)
            .IsRequired();
          
        builder.HasOne(x => x.FirstCompetitionRegister)
            .WithMany()
            .HasForeignKey(x => x.FirstCompetitionRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.SecondCompetitionRegister)
            .WithMany()
            .HasForeignKey(x => x.SecondCompetitionRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
