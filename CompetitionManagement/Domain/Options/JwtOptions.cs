namespace CompetitionManagement.Domain.Options;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public long ExpiryInMinutes { get; set; }
}