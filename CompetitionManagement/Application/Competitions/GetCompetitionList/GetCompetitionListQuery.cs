using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.GetCompetitionList;

public record GetCompetitionListQuery : IRequest<List<CompetitionDto>>;

public class GetCompetitionListQueryHandler(ApplicationDbContext dbContext) :
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
                s.Status))
            .ToListAsync(cancellationToken);
}