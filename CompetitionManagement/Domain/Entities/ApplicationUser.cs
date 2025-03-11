using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Domain.Entities;

public class ApplicationUser : IdentityUser
{   
    public ApplicationUser(string firstName, string lastName, string nationalId)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalId = nationalId;
    }

    protected ApplicationUser(
        string username,
        UserType type,
        string firstName,
        string lastName)
    {
        UserName = username;
        Type = type;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserType Type { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime? BirthDate { get; set; }
    public string? NationalId { get; private set; }
    
    public virtual List<Competition> CompetitionDefinitions { get; set; } = new();

    
    public static ApplicationUser Create(
        string phoneNumber,
        UserType type,
        string firstName,
        string lastName)
    {
        var user = new ApplicationUser(
            phoneNumber,
            type,
            firstName,
            lastName);

        new UserValidator().ValidateAndThrow(user);
        return user;
    }
}
