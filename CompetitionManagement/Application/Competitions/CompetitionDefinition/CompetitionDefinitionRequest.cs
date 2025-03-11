using CompetitionManagement.Application.Services;
using CompetitionManagement.Domain.Entities;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Application.Competitions;

public record CompetitionDefinitionRequest(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string CompetitionTitle,
    DateTime CompetitionDate,
    string CompetitionAddress,
    long LicenseFileId,
    long BannerFileId) : IRequest<CompetitionDefinitionResulDto>;

public class CompetitionDefinitionRequestHandler(
    ISmsService smsService,
    ITokenServices tokenServices,
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) :
    IRequestHandler<CompetitionDefinitionRequest, CompetitionDefinitionResulDto?>
{
    public async Task<CompetitionDefinitionResulDto?> Handle(CompetitionDefinitionRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is null)
        {
            user = ApplicationUser.Create(
                request.PhoneNumber,
                UserType.Planner,
                request.FirstName,
                request.LastName);

            await userManager.CreateAsync(user);
        }


        var competition = Competition.Create(
            user,
            request.CompetitionTitle,
            request.CompetitionDate,
            request.CompetitionAddress,
            request.BannerFileId,
            request.LicenseFileId);

        dbContext.Competitions.Add(competition);

        await dbContext.SaveChangesAsync(cancellationToken);

        var token = await tokenServices.GenerateToken(user.Id, competition.Id, TokenType.CompetitionDefinition);

        await smsService.SendOtp(user.UserName!, token!.OtpCode);

        return new CompetitionDefinitionResulDto(token.Token);
    }
}