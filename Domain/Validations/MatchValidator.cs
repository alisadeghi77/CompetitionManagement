using Domain.Entities;
using FluentValidation;

namespace Domain.Validations;

public class MatchValidator : AbstractValidator<Match>
{
    public MatchValidator()
    {
        RuleFor(x => x.KeyParams)
            .NotEmpty().WithMessage("دسته جدول الزامی است.");
    }
}