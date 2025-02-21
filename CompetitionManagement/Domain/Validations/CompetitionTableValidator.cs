using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class CompetitionTableValidator : AbstractValidator<Competition>
{
    public CompetitionTableValidator()
    {
        RuleFor(x => x.CompetitionDefinitionId)
            .GreaterThan(0).WithMessage(" مسابقه الزامی است.");

        RuleFor(x => x.AgeGroupId)
            .GreaterThan(0).WithMessage(" گروه سنی الزامی است.");

        RuleFor(x => x.Weight)
            .GreaterThan(0).WithMessage("وزن الزامی است.");

        RuleFor(x => x.Style)
            .NotEmpty().WithMessage("سبک‌ها الزامی هستند.");
    }
}
