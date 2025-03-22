using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CompetitionTableDetailsConfiguration : IEntityTypeConfiguration<CompetitionTable>
{
    public void Configure(EntityTypeBuilder<CompetitionTable> builder)
    {
        builder.ToTable("CompetitionTableDetails");

        builder.HasKey(ctd => ctd.Id);
        
        builder.Property(ctd => ctd.Id).ValueGeneratedOnAdd();

        builder.Property(ctd => ctd.FirstParticipantId)
            .IsRequired();

        builder.Property(ctd => ctd.SecondParticipantId)
            .IsRequired();

        builder.Property(ctd => ctd.Status)
            .IsRequired();
          
        builder.HasOne(x => x.FirstParticipant)
            .WithMany()
            .HasForeignKey(x => x.FirstParticipantId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        
        builder.HasOne(x => x.SecondParticipant)
            .WithMany()
            .HasForeignKey(x => x.SecondParticipantId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
