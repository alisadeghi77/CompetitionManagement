using Application.Common;
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
        var entity = await dbContext.Competitions
            .FirstOrDefaultAsync(w => w.Id == query.Id, cancellationToken);

        if (entity is null)
            throw new NotFoundException("مسابقه یافت نشد.");
        
        return new CompetitionDetailsDto(
            entity.Id,
            entity.Title,
            entity.Date,
            entity.Address,
            entity.BannerImageId,
            entity.LicenseImageId,
            entity.Status,
            entity.RegisterParams);
    }
}