using CompetitionManagement.Domain.Constant;
using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.GetCoaches;

public record GetCoachesQuery(string PhoneNumber) : IRequest<List<CoachDto>>;

public class GetCoachesQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetCoachesQuery, List<CoachDto>>
{
    public async Task<List<CoachDto>> Handle(GetCoachesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(w => w.Roles.Any(r => r.Name!.ToUpper() == RoleConstant.Coach))
            .Where(w => w.PhoneNumber!.Contains(request.PhoneNumber))
            .Select(s => new CoachDto(s.Id, s.FullName, s.UserName!))
            .ToListAsync(cancellationToken);
    }
}