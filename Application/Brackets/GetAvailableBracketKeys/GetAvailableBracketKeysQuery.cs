using MediatR;

namespace Application.Brackets.GetAvailableBracketKeys;

public record GetAvailableBracketKeysQuery(long CompetitionId) : IRequest<List<ParamsKeyDto>>;