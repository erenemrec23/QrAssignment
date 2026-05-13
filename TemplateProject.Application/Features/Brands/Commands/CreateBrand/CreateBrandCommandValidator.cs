using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.Features.Brands.Commands.UpdateBrand;
using TemplateProject.Application.Interfaces;

namespace TemplateProject.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        // IAppLocalizer'ı kullanarak JSON'dan dinamik çoklu dil mesajlarını çekiyoruz!
        public CreateBrandCommandValidator(IAppLocalizer localizer)
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["Validations.BrandNameRequired"])
                .MinimumLength(3).WithMessage(localizer["Validations.BrandNameMinLength"]);
        }
    }
}
