using Application.Common;
using Domain.Constant;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.GetParticipantList;

public record GetParticipantListQuery(long CompetitionId, string UserRequestingId, IEnumerable<string> UserRequestingRoles)
    : IRequest<List<ParticipantDto>>;

public class GetParticipantListQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetParticipantListQuery, List<ParticipantDto>>
{
    public async Task<List<ParticipantDto>> Handle(GetParticipantListQuery request, CancellationToken cancellationToken)
    {
        var isAdmin = request.UserRequestingRoles.Any(w => w.ToUpper() == RoleConstant.Coach);
        
        return await dbContext.Participants
            .Include(c => c.CoachUser)
            .Include(c => c.ParticipantUser)
            .Where(w => w.CompetitionId == request.CompetitionId &&
                        (isAdmin || w.Competition.PlannerUserId == request.UserRequestingId) &&
                        w.Competition.Status != CompetitionStatus.End)
            .Select(s => new ParticipantDto(
                s.Id,
                s.ParticipantUser.Id,
                s.ParticipantUser.FullName,
                s.ParticipantUser.PhoneNumber,
                s.CoachUserId,
                s.CoachUser!.FullName,
                s.CoachUser!.PhoneNumber,
                s.Status,
                s.RegisterParams))
            .ToListAsync(cancellationToken);
    }
}