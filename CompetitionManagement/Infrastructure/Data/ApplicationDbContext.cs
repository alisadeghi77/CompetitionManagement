using System.Reflection;
using CompetitionManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("xNpgsql.DisableDateTimeInfinityConversions", true);
    }

    public DbSet<AgeGroup> AgeGroups => Set<AgeGroup>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<CompetitionRegister> CompetitionRegisters => Set<CompetitionRegister>();
    public DbSet<CompetitionDetails> CompetitionTables => Set<CompetitionDetails>();
    public DbSet<CompetitionTableDetail> CompetitionTableDetails => Set<CompetitionTableDetail>();
    public DbSet<FileEntity> Files => Set<FileEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
