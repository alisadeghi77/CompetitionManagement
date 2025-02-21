using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class CompetitionValidator : AbstractValidator<Entities.CompetitionDefinition>
{
    public CompetitionValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان مسابقه الزامی است.")
            .MaximumLength(200).WithMessage("عنوان نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now).WithMessage("تاریخ مسابقه باید در آینده باشد.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("آدرس الزامی است.")
            .MaximumLength(300).WithMessage("آدرس نمی‌تواند بیشتر از ۳۰۰ کاراکتر باشد.");
    }
}
