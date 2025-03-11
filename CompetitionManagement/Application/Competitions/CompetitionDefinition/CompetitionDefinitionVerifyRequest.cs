using CompetitionManagement.Application.Services;
using CompetitionManagement.Domain.Entities;
using CompetitionManagement.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CompetitionManagement.Application.Competitions;

public record CompetitionDefinitionVerifyRequest(string Token, string OtpCode) : IRequest;

public class CompetitionDefinitionVerifyRequestHandler(
    ISmsService smsService,
    ITokenServices tokenServices,
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager):
    IRequestHandler<CompetitionDefinitionVerifyRequest>
{
    public async Task Handle(CompetitionDefinitionVerifyRequest request, CancellationToken cancellationToken)
    {
        var tokenDto = tokenServices.ValidateToken(request.Token, request.OtpCode);
        if (tokenDto is null)
            throw new Exception("کد نامعتبر است.");

        var competition = await dbContext.Competitions
            .Where(w => w.Id == tokenDto.CompetitionId && w.PlannerUserId == tokenDto.PlannerUserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (competition is null)
        {
            var exception = new Exception("کد نامعتبر است.");
            exception.Data.Add("TokenDto",tokenDto);
            throw exception;
        }

        competition.Verify();

        dbContext.Entry(competition);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
