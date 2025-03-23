using System.Security.Claims;

namespace Application.Common;

public interface ITokenService
{
    string GenerateToken(IEnumerable<Claim> claims);
}