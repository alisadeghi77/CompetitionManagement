using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class AgeGroupValidator : AbstractValidator<AgeGroup>
{
    public AgeGroupValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان گروه سنی الزامی است.")
            .MaximumLength(100).WithMessage("عنوان گروه نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.Weights)
            .NotEmpty().WithMessage("وزن‌ها الزامی هستند.")
            .Must(w => w.Count > 0).WithMessage("حداقل یک وزن باید تعریف شود.");

        RuleFor(x => x.Styles)
            .NotEmpty().WithMessage("سبک‌ها الزامی هستند.")
            .Must(s => s.Count > 0).WithMessage("حداقل یک سبک باید تعریف شود.");
    }
}
