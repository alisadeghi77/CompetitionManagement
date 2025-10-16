using Application.Common;
using Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brackets.DeleteBracketByKey;

public record DeleteBracketByKeyCommand(long CompetitionId, string BracketKey) : IRequest;

public class DeleteBracketByKeyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteBracketByKeyCommand>
{
    public async Task Handle(DeleteBracketByKeyCommand request, CancellationToken cancellationToken)
    {
        var bracket = await dbContext.Brackets
            .Include(c => c.Matches)
            .FirstOrDefaultAsync(w =>
                w.CompetitionId == request.CompetitionId && w.KeyParams.Equals(request.BracketKey), cancellationToken);

        if (bracket is null)
            return;

        dbContext.RemoveRange(bracket.Matches.ToArray());
        dbContext.Remove(bracket);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}