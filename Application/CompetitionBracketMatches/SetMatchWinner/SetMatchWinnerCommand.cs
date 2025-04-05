using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompetitionBracketMatches.SetMatchWinner;

public record SetMatchWinnerCommand(Guid MatchId, long ParticipantId) : IRequest;

public class SetMatchWinnerCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<SetMatchWinnerCommand>
{
    public async Task Handle(SetMatchWinnerCommand request, CancellationToken cancellationToken)
    {
        var match = await dbContext.CompetitionBracketMatches
            .Include(c => c.FirstParticipant)
            .Include(c => c.SecondParticipant)
            .Include(c => c.NextMatch)
            .Include(c => c.WinnerParticipant)
            .FirstOrDefaultAsync(w => w.Id == request.MatchId, cancellationToken);

        if (match is null)
            throw new UnprocessableEntityException("مسابقه مورد نظر یافت نشد.");

        if (match.FirstParticipantId == request.ParticipantId)
            match.SetFirstParticipantWinner();
        else if (match.SecondParticipantId == request.ParticipantId)
            match.SetSecondParticipantWinner();
        else
            throw new UnprocessableEntityException("شرکت کننده مورد نظر برای این بازی یافت نشد.");

        dbContext.CompetitionBracketMatches.Update(match);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}