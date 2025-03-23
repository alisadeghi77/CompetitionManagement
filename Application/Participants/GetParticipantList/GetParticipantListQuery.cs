using Application.Common;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Participants.GetParticipantList;

public record GetParticipantListQuery(string PlannerUserId) : IRequest<List<ParticipantDto>>;

public class GetParticipantListQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetParticipantListQuery, List<ParticipantDto>>
{
    public async Task<List<ParticipantDto>> Handle(GetParticipantListQuery request, CancellationToken cancellationToken)
        => await dbContext.Participants
            .Include(c => c.CoachUser)
            .Include(c => c.ParticipantUser)
            .Where(w => w.Competition.PlannerUserId == request.PlannerUserId &&
                        w.Competition.Status != CompetitionStatus.End)
            .Select(s => new ParticipantDto(
                s.Id,
                s.ParticipantUser.Id,
                s.ParticipantUser.FirstName,
                s.ParticipantUser.LastName,
                s.ParticipantUser.PhoneNumber,
                s.CoachUserId,
                s.CoachUser!.FirstName,
                s.CoachUser!.LastName,
                s.CoachUser!.PhoneNumber,
                s.Status,
                s.RegisterParams))
            .ToListAsync(cancellationToken);
}