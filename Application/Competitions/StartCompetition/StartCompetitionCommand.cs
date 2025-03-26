using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.StartCompetition;

public record StartCompetitionCommand(long Id) : IRequest;

public class StartCompetitionCommandHandler(IApplicationDbContext dbContext) 
    : IRequestHandler<StartCompetitionCommand>
{
    public async Task Handle(StartCompetitionCommand request, CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (competition is null)
            throw new NotFoundException("مسابقه یافت نشد.");
        
        competition.SetApprove();
        competition.ChangeVisibility(true);
        competition.ChangeRegistrationStatus(true);

        dbContext.Competitions.Update(competition);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}