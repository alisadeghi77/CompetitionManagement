using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class CompetitionTableDetailsValidator : AbstractValidator<CompetitionTable>
{
    public CompetitionTableDetailsValidator()
    {
        RuleFor(x => x.FirstCompetitionRegisterId)
            .GreaterThan(0).WithMessage("شناسه ثبت‌نام اول الزامی است.");

        RuleFor(x => x.SecondCompetitionRegisterId)
            .GreaterThan(0).WithMessage("شناسه ثبت‌نام دوم الزامی است.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("وضعیت معتبر نیست.");
    }
}