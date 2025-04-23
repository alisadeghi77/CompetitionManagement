using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Common;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<IdentityRole> Roles { get; }
    DbSet<Competition> Competitions { get; }
    DbSet<Participant> Participants { get; }
    DbSet<Bracket> Brackets { get; }
    DbSet<Match> Matches { get; }
    DbSet<FileEntity> Files { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    void RemoveRange(params object[] entities);
    EntityEntry<TEntity> Remove<TEntity>(TEntity entity) where TEntity : class;
}