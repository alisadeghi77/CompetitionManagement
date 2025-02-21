using CompetitionManagement.Domain.Common;
using CompetitionManagement.Domain.Enums;
using CompetitionManagement.Domain.Validations;
using FluentValidation;

namespace CompetitionManagement.Domain.Entities;

public class CompetitionRegister : BaseAuditableEntity
{
    public required string AthleteUserId { get; set; }

    public required ApplicationUser AthleteUser { get; set; }

    public required string CoachPhoneNumber { get; set; }
    
    public string? CoachUserId { get; private set; }
    
    public ApplicationUser? CoachUser { get; private set; }

    public RegisterStatus Status { get; private set; }
    
    public long AgeGroupId { get; private set; }
    
    public required AgeGroup AgeGroup { get; set; }
    
    public int Weight { get; private set; }
    
    public required string Style { get; set; }


    public static CompetitionRegister Create(
        ApplicationUser athleteUser,
        ApplicationUser coachUser,
        string coachPhoneNumber,
        RegisterStatus status,
        AgeGroup ageGroup,
        int weight,
        string styles)
    {
        var register = new CompetitionRegister
        {
            Style = styles,
            AthleteUser = athleteUser,
            AthleteUserId = athleteUser.Id,
            CoachPhoneNumber = coachPhoneNumber,
            AgeGroup = ageGroup,
            AgeGroupId = ageGroup.Id,
            CoachUserId = coachUser.Id,
            CoachUser = coachUser,
            Status = status,
            Weight = weight,
        };

        new CompetitionRegisterValidator().ValidateAndThrow(register);
        return register;
    }
}
