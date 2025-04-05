using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.KeyParams)
            .IsRequired();

        builder.Property(x => x.Round)
            .IsRequired();

        builder.Property(x => x.MatchNumberPosition)
            .IsRequired();

        builder.HasOne(x => x.Bracket)
            .WithMany(x => x.Matches)
            .HasForeignKey(x => x.BracketId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(x => x.FirstParticipant)
            .WithMany()
            .HasForeignKey(x => x.FirstParticipantId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasOne(x => x.SecondParticipant)
            .WithMany()
            .HasForeignKey(x => x.SecondParticipantId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);

        builder.HasOne(x => x.WinnerParticipant)
            .WithMany()
            .HasForeignKey(x => x.WinnerParticipantId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
        
        builder.HasOne(x => x.NextMatch)
            .WithMany()
            .HasForeignKey(x => x.NextMatchId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);
    }
}