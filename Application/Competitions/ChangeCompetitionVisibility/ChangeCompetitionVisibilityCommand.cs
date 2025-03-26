
using Application.Common;
using Application.Competitions.StartCompetition;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.ChangeCompetitionVisibility;

public record ChangeCompetitionVisibilityCommand(long Id) : IRequest;

public class ChangeCompetitionVisibilityCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ChangeCompetitionVisibilityCommand>
{
    public async Task Handle(ChangeCompetitionVisibilityCommand request, CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (competition is null)
            throw new NotFoundException("مسابقه یافت نشد.");
        
        competition.ChangeVisibility(!competition.IsVisible);

        dbContext.Competitions.Update(competition);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}