namespace Domain.Contracts;

public interface ICurrentUser
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    IEnumerable<string> Roles { get; }
    string? GetClaim(string claimType);
}
