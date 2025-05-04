using System.Text.Json;
using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.GetCompetitionById;

public record GetCompetitionByIdQuery(long Id) : IRequest<CompetitionDetailsDto?>;

public class GetCompetitionByIdQueryHandler(IApplicationDbContext dbContext) :
    IRequestHandler<GetCompetitionByIdQuery, CompetitionDetailsDto?>
{
    public async Task<CompetitionDetailsDto?> Handle(GetCompetitionByIdQuery query, CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == query.Id, cancellationToken);

        if (competition is null)
            throw new NotFoundException("مسابقه یافت نشد.");

        return new CompetitionDetailsDto(
            competition.Id,
            competition.Title,
            competition.Date,
            competition.Address,
            competition.BannerImageId,
            competition.LicenseImageId,
            competition.Status,
            competition.IsVisible,
            competition.CanRegister,
            competition.RegisterParams);
    }
}