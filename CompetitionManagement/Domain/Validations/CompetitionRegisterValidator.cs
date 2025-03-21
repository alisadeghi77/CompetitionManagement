using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class CompetitionRegisterValidator : AbstractValidator<CompetitionRegister>
{
    public CompetitionRegisterValidator()
    {
        RuleFor(x => x.AthleteUserId)
            .NotEmpty().NotNull().WithMessage("شناسه ورزشکار الزامی است.");

        RuleFor(x => x.CoachUserId)
            .NotEmpty().NotNull().WithMessage("شناسه مربی الزامی است.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت ثبت نام معتبر نیست.");
    }
}
