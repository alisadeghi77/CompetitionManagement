using System.Security.Claims;
using Domain.Contracts;

namespace Api.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal _user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public string? UserId => _user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? UserName => _user.Identity?.Name;
    public string? Email => _user.FindFirst(ClaimTypes.Email)?.Value;

    public IEnumerable<string> Roles => 
        _user.FindAll(ClaimTypes.Role).Select(r => r.Value);

    public string? GetClaim(string claimType) => 
        _user.FindFirst(claimType)?.Value;
}