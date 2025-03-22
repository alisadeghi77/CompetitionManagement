using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Competition> Competitions { get; }
    DbSet<CompetitionRegister> CompetitionRegisters { get; }
    DbSet<CompetitionTable> CompetitionTables { get; }
    DbSet<FileEntity> Files { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}