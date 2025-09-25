using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Competitions.CompetitionDefinition;

public record CompetitionDefinitionCommand(
    string UserId,
    string Title,
    DateTime Date,
    string Address,
    long LicenseImageId,
    long BannerImageId) : IRequest<long>;

public class CompetitionDefinitionCommandHandler(
    IApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<CompetitionDefinitionCommand, long>
{
    public async Task<long> Handle(CompetitionDefinitionCommand command,
        CancellationToken cancellationToken)
    {
        var plannerUser = await userManager.FindByIdAsync(command.UserId);
        if (plannerUser is null)
            throw new UnprocessableEntityException("کاربر یافت نشد");

        var competition = Competition.Create(
            plannerUser,
            command.Title,
            command.Date,
            command.Address,
            command.BannerImageId,
            command.LicenseImageId);

        dbContext.Competitions.Add(competition);

        await dbContext.SaveChangesAsync(cancellationToken);

        return competition.Id;
    }
}