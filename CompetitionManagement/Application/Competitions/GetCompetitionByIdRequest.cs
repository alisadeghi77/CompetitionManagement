using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions;

public record GetCompetitionByIdRequest(long Id) : IRequest<CompetitionDetailsDto?>;

public class GetCompetitionByIdRequestHandler(ApplicationDbContext dbContext) :
        IRequestHandler<GetCompetitionByIdRequest, CompetitionDetailsDto?>
{
        public async Task<CompetitionDetailsDto?> Handle(GetCompetitionByIdRequest request,
                CancellationToken cancellationToken) =>
                await dbContext.CompetitionTables
                        .Select(s => new CompetitionDetailsDto(
                                s.Id,
                                s.CompetitionDefinitionId,
                                s.AgeGroupId,
                                s.Weight,
                                s.Style))
                        .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);
}