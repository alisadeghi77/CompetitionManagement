using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.GetCompetitionById;

public record GetCompetitionByIdQuery(long Id) : IRequest<CompetitionDetailsDto?>;

public class GetCompetitionByIdQueryHandler(ApplicationDbContext dbContext) :
    IRequestHandler<GetCompetitionByIdQuery, CompetitionDetailsDto?>
{
    public async Task<CompetitionDetailsDto?> Handle(
        GetCompetitionByIdQuery query, CancellationToken cancellationToken) => await dbContext.Competitions
        .Select(s => new CompetitionDetailsDto(
            s.Id,
            s.Title,
            s.Date,
            s.Address,
            s.BannerImageId,
            s.LicenseImageId,
            s.Status,
            s.RegisterParams))
        .FirstOrDefaultAsync(w => w.Id == query.Id, cancellationToken);
}