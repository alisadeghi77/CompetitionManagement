using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class BracketConfiguration : IEntityTypeConfiguration<Bracket>
{
    public void Configure(EntityTypeBuilder<Bracket> builder)
    {
        builder.ToTable("Brackets");

        builder.HasKey(ctd => ctd.Id);
        
        builder.Property(ctd => ctd.Id).ValueGeneratedOnAdd();

        builder.Property(c => c.RegisterParams)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<ParticipantParam>?>(v, (JsonSerializerOptions)null));
        
        
        builder.HasOne(x => x.Competition)
            .WithMany(x => x.Brackets)
            .HasForeignKey(x => x.CompetitionId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
        
        builder.HasOne(c => c.GoldMedalistParticipant)
            .WithMany()
            .HasForeignKey(c => c.GoldMedalistParticipantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.SilverMedalistParticipant)
            .WithMany()
            .HasForeignKey(c => c.SilverMedalistParticipantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.BronzeMedalistParticipant)
            .WithMany()
            .HasForeignKey(c => c.BronzeMedalistParticipantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.JoinBronzeMedalistParticipant)
            .WithMany()
            .HasForeignKey(c => c.JoinBronzeMedalistParticipantId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
