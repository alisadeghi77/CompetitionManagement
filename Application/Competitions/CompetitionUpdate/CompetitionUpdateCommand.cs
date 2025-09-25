using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Competitions.CompetitionUpdate;

public record CompetitionUpdateCommand(
    int Id,
    string Title,
    DateTime Date,
    string Address,
    long LicenseImageId,
    long BannerImageId) : IRequest<long>;

public class CompetitionUpdateCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CompetitionUpdateCommand, long>
{
    public async Task<long> Handle(CompetitionUpdateCommand command, CancellationToken cancellationToken)
    {
        var competition = await dbContext.Competitions.FirstOrDefaultAsync(w => w.Id == command.Id, cancellationToken);
        if (competition is null)
            throw new UnprocessableEntityException("مسابقه مورد نظر یافت نشد.");

        competition.Update(
                     command.Title,
                     command.Date,
                     command.Address,
                     command.BannerImageId,
                     command.LicenseImageId);

        dbContext.Competitions.Update(competition);

        await dbContext.SaveChangesAsync(cancellationToken);

        return competition.Id;
    }
}