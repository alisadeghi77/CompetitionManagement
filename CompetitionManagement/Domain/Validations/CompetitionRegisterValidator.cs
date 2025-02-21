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

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("وزن الزامی است.");

        RuleFor(x => x.Style)
            .NotEmpty().WithMessage("سبک‌ الزامی هستند.");
    }
}
