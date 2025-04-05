using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GetCoachMedalReport;

public record GetCoachMedalReportQuery(long CompetitionId) : IRequest<object>;

public class GetCoachMedalReportQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCoachMedalReportQuery, object>
{
    public async Task<object> Handle(GetCoachMedalReportQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Brackets
            .Include(c => c.GoldMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.SilverMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.BronzeMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Include(c => c.JoinBronzeMedalistParticipant)
            .ThenInclude(w => w.CoachUser)
            .Where(w => w.CompetitionId == request.CompetitionId)
            .SelectMany(bracket => new[]
            {
                new
                {
                    bracket.GoldMedalistParticipant.CoachUser.Id,
                    bracket.GoldMedalistParticipant.CoachUser.FullName,
                    Rank = 1,
                    Score = 4,
                },
                new
                {
                    bracket.SilverMedalistParticipant.CoachUser.Id,
                    bracket.SilverMedalistParticipant.CoachUser.FullName,
                    Rank = 2,
                    Score = 3,
                },
                new
                {
                    bracket.BronzeMedalistParticipant.CoachUser.Id,
                    bracket.BronzeMedalistParticipant.CoachUser.FullName,
                    Rank = 3,
                    Score = 2,
                },
                new
                {
                    bracket.JoinBronzeMedalistParticipant.CoachUser.Id,
                    bracket.JoinBronzeMedalistParticipant.CoachUser.FullName,
                    Rank = 4,
                    Score = 1,
                }
            })
            .GroupBy(g => new { g.Id, g.FullName })
            .Select(s => new
            {
                s.Key.Id,
                s.Key.FullName,
                Gold = s.Count(w => w.Rank == 1),
                Silver = s.Count(w => w.Rank == 2),
                Bronze = s.Count(w => w.Rank == 3),
                JoinBronze = s.Count(w => w.Rank == 4),
                Score = s.Sum(s => s.Score)
            })
            .ToListAsync(cancellationToken);
    }
}