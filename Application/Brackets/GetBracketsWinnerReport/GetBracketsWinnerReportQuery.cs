using System.Reflection.Metadata.Ecma335;
using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GetBracketsWinnerReport;

public record GetBracketsWinnerReportQuery(long CompetitionId) : IRequest<object>;

public class GetBracketsWinnerReportQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetBracketsWinnerReportQuery, object>
{
    public async Task<object> Handle(GetBracketsWinnerReportQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Brackets
            .Include(c => c.GoldMedalistParticipant)
            .ThenInclude(w => w.ParticipantUser)
            .Include(c => c.GoldMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.SilverMedalistParticipant)
            .ThenInclude(w => w.ParticipantUser)
            .Include(c => c.SilverMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.BronzeMedalistParticipant)
            .ThenInclude(w => w.ParticipantUser)
            .Include(c => c.BronzeMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.JoinBronzeMedalistParticipant)
            .ThenInclude(w => w.ParticipantUser)
            .Include(c => c.JoinBronzeMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Where(w => w.CompetitionId == request.CompetitionId)
            .Select(s => new
            {
                s.Id,
                s.KeyParams,
                s.RegisterParams,
                GoldParticipantUser = s.GoldMedalistParticipant!.ParticipantUser.FullName,
                GoldCoachUser = s.GoldMedalistParticipant.CoachUser.FullName,

                SilverParticipantUser = s.SilverMedalistParticipant!.ParticipantUser.FullName,
                SilverCoachUser = s.SilverMedalistParticipant.CoachUser.FullName,

                BronzeParticipantUser = s.BronzeMedalistParticipant!.ParticipantUser.FullName,
                BronzeCoachUser = s.BronzeMedalistParticipant.CoachUser.FullName,

                JoinBronzeParticipantUser = s.JoinBronzeMedalistParticipant!.ParticipantUser.FullName,
                JoinBronzeCoachUser = s.JoinBronzeMedalistParticipant.CoachUser.FullName
            })
            .ToListAsync(cancellationToken);
    }
}