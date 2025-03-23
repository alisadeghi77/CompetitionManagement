using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.UpdateParams;

public record UpdateCompetitionParamsCommand(long CompetitionId, List<CompetitionParam> RegisterParams) : IRequest;

public class UpdateCompetitionParamsCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateCompetitionParamsCommand>
{
    public async Task Handle(UpdateCompetitionParamsCommand request, CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("مسابقه یافت نشد");

        competition.SetRegisterParams(request.RegisterParams);
        
        competition.SetApprove();
        
        dbContext.Competitions.Update(competition);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}