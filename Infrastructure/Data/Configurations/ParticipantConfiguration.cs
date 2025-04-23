using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("Participants");

        builder.HasKey(cr => cr.Id);
        
        builder.Property(cr => cr.Id).ValueGeneratedOnAdd();


        builder.Property(cr => cr.Status)
            .IsRequired();

        builder.HasOne(x => x.ParticipantUser)
            .WithMany()
            .HasForeignKey(x => x.ParticipantUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
            
        builder.HasOne(x => x.CoachUser)
            .WithMany()
            .HasForeignKey(x => x.CoachUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        
        builder.Property(c => c.RegisterParams)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<ParticipantParam>>(v, (JsonSerializerOptions)null));

    }
}
