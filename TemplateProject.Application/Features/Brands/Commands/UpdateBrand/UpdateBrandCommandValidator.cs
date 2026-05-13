namespace QrAssignment.Application.Features.Brands.Commands.UpdateBrand
{
    using FluentValidation;
    using QrAssignment.Application.Interfaces;

    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        // IAppLocalizer'ı kullanarak JSON'dan dinamik çoklu dil mesajlarını çekiyoruz!
        public UpdateBrandCommandValidator(IAppLocalizer localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["Validations.BrandNameRequired"])
                .MinimumLength(3).WithMessage(localizer["Validations.BrandNameMinLength"]);
        }
    }
}
