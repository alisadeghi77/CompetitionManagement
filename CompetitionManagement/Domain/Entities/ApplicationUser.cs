using System.ComponentModel.DataAnnotations.Schema;
using CompetitionManagement.Domain.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CompetitionManagement.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser( string phoneNumber,string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        UserName = phoneNumber;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime? BirthDate { get; set; }
    public string? NationalId { get; private set; }

    public virtual List<Competition> CompetitionDefinitions { get; set; } = new();


    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";


    public static ApplicationUser Create(
        string phoneNumber,
        string firstName,
        string lastName)
    {
        var user = new ApplicationUser(
            phoneNumber,
            firstName,
            lastName);

        new UserValidator().ValidateAndThrow(user);
        return user;
    }
}