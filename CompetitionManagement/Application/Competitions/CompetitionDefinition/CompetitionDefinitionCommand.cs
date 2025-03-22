using CompetitionManagement.Application.Services;
using CompetitionManagement.Domain.Entities;
using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Application.Competitions.CompetitionDefinition;

public record CompetitionDefinitionCommand(
    string UserId,
    string CompetitionTitle,
    DateTime CompetitionDate,
    string CompetitionAddress,
    long LicenseFileId,
    long BannerFileId) : IRequest<long>;

public class CompetitionDefinitionCommandHandler(
    ISmsService smsService,
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<CompetitionDefinitionCommand, long>
{
    public async Task<long> Handle(CompetitionDefinitionCommand command,
        CancellationToken cancellationToken)
    {
        var plannerUser = await userManager.FindByIdAsync(command.UserId);
        if (plannerUser is null)
            throw new Exception("کاربر یافت نشد");

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