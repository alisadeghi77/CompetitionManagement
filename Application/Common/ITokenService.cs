using System.Security.Claims;

namespace Application.Services;

public interface ITokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
}