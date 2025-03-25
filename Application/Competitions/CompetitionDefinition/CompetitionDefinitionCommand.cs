using Application.Common;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Competitions.CompetitionDefinition;

public record CompetitionDefinitionCommand(
    string UserId,
    string CompetitionTitle,
    DateTime CompetitionDate,
    string CompetitionAddress,
    long LicenseFileId,
    long BannerFileId) : IRequest<long>;

public class CompetitionDefinitionCommandHandler(
    ISmsService smsService,
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
            command.CompetitionTitle,
            command.CompetitionDate,
            command.CompetitionAddress,
            command.BannerFileId,
            command.LicenseFileId);

        dbContext.Competitions.Add(competition);

        await dbContext.SaveChangesAsync(cancellationToken);

        return competition.Id;
    }
}