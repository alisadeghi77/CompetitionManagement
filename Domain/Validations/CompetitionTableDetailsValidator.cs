using Domain.Entities;
using FluentValidation;

namespace Domain.Validations;

public class CompetitionTableDetailsValidator : AbstractValidator<CompetitionTable>
{
    public CompetitionTableDetailsValidator()
    {
        RuleFor(x => x.FirstPlayerRegisterId)
            .GreaterThan(0).WithMessage("شناسه ثبت‌نام اول الزامی است.");

        RuleFor(x => x.SecondPlayerRegisterId)
            .GreaterThan(0).WithMessage("شناسه ثبت‌نام دوم الزامی است.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت معتبر نیست.");
    }
}