namespace Application.Auth.CurrentUser;

public record UserDto(string Id, string UserName, string PhoneNumber, string FullName, List<string> Roles);