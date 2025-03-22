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

        builder.Property(ctd => ctd.FirstPlayerRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.SecondPlayerRegisterId)
            .IsRequired();

        builder.Property(ctd => ctd.Status)
            .IsRequired();
          
        builder.HasOne(x => x.FirstPlayerRegister)
            .WithMany()
            .HasForeignKey(x => x.FirstPlayerRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.SecondPlayerRegister)
            .WithMany()
            .HasForeignKey(x => x.SecondPlayerRegisterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
