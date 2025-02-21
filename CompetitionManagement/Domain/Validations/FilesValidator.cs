using CompetitionManagement.Domain.Entities;
using FluentValidation;

namespace CompetitionManagement.Domain.Validations;

public class FilesValidator : AbstractValidator<FileEntity>
{
    public FilesValidator()
    {
        RuleFor(x => x.Base64Content)
            .NotEmpty().WithMessage("محتوای فایل به صورت Base64 الزامی است.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage("نام فایل الزامی است.")
            .MaximumLength(200).WithMessage("نام فایل نمی‌تواند بیشتر از ۲۰۰ کاراکتر باشد.");
    }
}
