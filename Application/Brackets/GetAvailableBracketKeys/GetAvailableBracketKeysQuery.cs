using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GetAvailableBracketKeys;

public record GetAvailableBracketKeysQuery(long CompetitionId) : IRequest<List<string>>;

public class GetAvailableBracketKeysQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAvailableBracketKeysQuery, List<string>>
{
    public async Task<List<string>> Handle(
        GetAvailableBracketKeysQuery request,
        CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .Include(c => c.Brackets)
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("مسابقه مورد نظر یافت نشد.");

        return competition.Brackets.Select(b => b.KeyParams).ToList();
    }
} 