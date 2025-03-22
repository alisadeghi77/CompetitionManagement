using Domain.Entities;
using FluentValidation;

namespace Domain.Validations;

public class ParticipantValidator : AbstractValidator<Participant>
{
    public ParticipantValidator()
    {
        //TODO: validate all params based on competitions
        
        RuleFor(x => x.ParticipantUserId)
            .NotEmpty().NotNull().WithMessage("شناسه ورزشکار الزامی است.");

        RuleFor(x => x.CoachUserId)
            .NotEmpty().NotNull().WithMessage("شناسه مربی الزامی است.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت ثبت نام معتبر نیست.");
    }
}
