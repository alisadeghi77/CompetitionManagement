using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.GetAvailableBracketKeys;

public class GetAvailableBracketKeysQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAvailableBracketKeysQuery, List<ParamsKeyDto>>
{
    public async Task<List<ParamsKeyDto>> Handle(
        GetAvailableBracketKeysQuery request,
        CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .Include(c => c.Brackets)
            .FirstOrDefaultAsync(w => w.Id == request.CompetitionId, cancellationToken);

        if (competition is null)
            throw new UnprocessableEntityException("مسابقه مورد نظر یافت نشد.");

        return competition
            .GetParamsWithKeys()
            .Select(s => new ParamsKeyDto(s.Key, competition.Brackets.Any(w => w.KeyParams == s.Key)))
            .ToList();
    }
}