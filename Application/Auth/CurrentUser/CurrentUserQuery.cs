using System.Security.Claims;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Auth.CurrentUser;

//TODO: change result to dto
public record GetCurrentUserQuery(ClaimsPrincipal User) : IRequest<UserDto>;

public class GetCurrentUserQueryHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (!request.User.Identity?.IsAuthenticated ?? true)
            throw new UnauthorizedException("کاربر احراز هویت نشده است");

        var user = await userManager.GetUserAsync(request.User);
        if (user == null)
            throw new NotFoundException("کاربر یافت نشد");

        var roles = await userManager.GetRolesAsync(user);

        return new UserDto(
            user.Id,
            user.UserName!,
            user.PhoneNumber!,
            user.FullName,
            roles.ToList());
    }
}