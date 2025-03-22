using Domain.Entities;
using FluentValidation;

namespace Domain.Validations;

public class CompetitionTableDetailsValidator : AbstractValidator<CompetitionTable>
{
    public CompetitionTableDetailsValidator()
    {
        RuleFor(x => x.FirstParticipantId)
            .GreaterThan(0).WithMessage("شناسه ثبت ‌نام اول الزامی است.");

        RuleFor(x => x.SecondParticipantId)
            .GreaterThan(0).WithMessage("شناسه ثبت ‌نام دوم الزامی است.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت معتبر نیست.");
    }
}