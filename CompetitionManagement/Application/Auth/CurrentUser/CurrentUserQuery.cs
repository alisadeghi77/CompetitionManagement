using System.Security.Claims;
using CompetitionManagement.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Application.Auth.CurrentUser;

//TODO: change result to dto
public record GetCurrentUserQuery(ClaimsPrincipal User) : IRequest<ApplicationUser?>;

public class GetCurrentUserQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetCurrentUserQuery, ApplicationUser?>
{
    public async Task<ApplicationUser?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken) 
        => await userManager.GetUserAsync(request.User);
}
