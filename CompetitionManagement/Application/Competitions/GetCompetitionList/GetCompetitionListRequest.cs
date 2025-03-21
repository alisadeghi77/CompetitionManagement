using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.GetCompetitionList;

public record GetCompetitionListRequest : IRequest<List<CompetitionDto>>;

public class GetCompetitionListRequestHandler(ApplicationDbContext dbContext) :
    IRequestHandler<GetCompetitionListRequest, List<CompetitionDto>>
{
    public async Task<List<CompetitionDto>> Handle(
        GetCompetitionListRequest request, CancellationToken cancellationToken) =>
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