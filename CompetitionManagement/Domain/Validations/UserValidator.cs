using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class UserValidator : AbstractValidator<ApplicationUser>
{
    public UserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("نام کاربری الزامی است.")
            .MaximumLength(50).WithMessage("نام کاربری نمی‌تواند بیشتر از ۵۰ کاراکتر باشد.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("شماره تلفن الزامی است.")
            .Matches(@"^\d+$").WithMessage("شماره تلفن فقط باید شامل اعداد باشد.")
            .Length(10, 15).WithMessage("شماره تلفن باید بین ۱۰ تا ۱۵ رقم باشد.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("نام الزامی است.")
            .MaximumLength(100).WithMessage("نام نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("نام خانوادگی الزامی است.")
            .MaximumLength(100).WithMessage("نام خانوادگی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("کد ملی الزامی است.")
            .Length(10).WithMessage("کد ملی باید ۱۰ رقم باشد.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now).WithMessage("تاریخ تولد باید قبل از تاریخ فعلی باشد.");
    }
}
