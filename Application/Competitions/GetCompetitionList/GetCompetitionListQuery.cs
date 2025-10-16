using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.GetCompetitionList;

public record GetCompetitionListQuery : IRequest<List<CompetitionDto>>;

public class GetCompetitionListQueryHandler(IApplicationDbContext dbContext) :
    IRequestHandler<GetCompetitionListQuery, List<CompetitionDto>>
{
    public async Task<List<CompetitionDto>> Handle(
        GetCompetitionListQuery query, CancellationToken cancellationToken) =>
        await dbContext.Competitions
            .Select(s => new CompetitionDto(
                s.Id,
                s.Title,
                s.Date,
                s.Address,
                s.BannerImageId,
                s.LicenseImageId,
                s.CanRegister,
                s.Status))
            .ToListAsync(cancellationToken);
}