using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions.GetCompetitionById;

public record GetCompetitionByIdRequest(long Id) : IRequest<CompetitionDetailsDto?>;

public class GetCompetitionByIdRequestHandler(ApplicationDbContext dbContext) :
    IRequestHandler<GetCompetitionByIdRequest, CompetitionDetailsDto?>
{
    public async Task<CompetitionDetailsDto?> Handle(
        GetCompetitionByIdRequest request, CancellationToken cancellationToken) => await dbContext.Competitions
        .Select(s => new CompetitionDetailsDto(
            s.Id,
            s.Title,
            s.Date,
            s.Address,
            s.BannerImageId,
            s.LicenseImageId,
            s.Status,
            s.RegisterParams))
        .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
}