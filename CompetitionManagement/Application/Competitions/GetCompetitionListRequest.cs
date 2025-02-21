using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions;

public record GetCompetitionListRequest : IRequest<List<CompetitionDto>>;

public class GetCompetitionListRequestHandler(ApplicationDbContext dbContext) :
        IRequestHandler<GetCompetitionListRequest, List<CompetitionDto>>
{
        public async Task<List<CompetitionDto>> Handle(
                GetCompetitionListRequest request,
                CancellationToken cancellationToken) =>
                await dbContext.CompetitionTables
                        .Select(s => new CompetitionDto(
                                s.Id,
                                s.CompetitionDefinitionId,
                                s.AgeGroupId,
                                s.Weight,
                                s.Style))
                        .ToListAsync(cancellationToken);
}