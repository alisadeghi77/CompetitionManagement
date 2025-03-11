using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Caching.Memory;

namespace CompetitionManagement.Application.Services;

public interface ITokenServices
{
    Task<TokenDto?> GenerateToken(string plannerUserId, long competitionId, TokenType type);
    TokenDto? ValidateToken(string token, string otpCode);
}

public class TokenServices(IMemoryCache memoryCache) : ITokenServices
{
    public async Task<TokenDto?> GenerateToken(string plannerUserId, long competitionId, TokenType type)
    {
        var key = $"{plannerUserId}-{competitionId}";

        return await memoryCache.GetOrCreateAsync<TokenDto>(key,
                   f =>
                   {
                       f.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                       return Task.FromResult(new TokenDto(key, 
                           plannerUserId,
                           competitionId,
                           new Random().Next(1000, 9999).ToString(), 
                           type));
                   }) ??
               throw new Exception("Generate token throw exception");
    }

    public TokenDto? ValidateToken(string token, string otpCode)
    {
        try
        {
            var tokenDto = memoryCache.Get<TokenDto>(token);
            if (tokenDto is not null && tokenDto.OtpCode == otpCode)
            {
                memoryCache.Remove(token);
                return tokenDto;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}

public record TokenDto(string Token, string PlannerUserId, long CompetitionId, string OtpCode, TokenType Type);

public enum TokenType
{
    CompetitionDefinition,
    CompetitionRegister
}