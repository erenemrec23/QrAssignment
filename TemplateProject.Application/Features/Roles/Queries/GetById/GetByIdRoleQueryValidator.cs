using FluentValidation;
using Microsoft.Extensions.Localization; // Kendi yapına göre namespace değişebilir

namespace QrAssignment.Application.Features.Roles.Queries.GetById
{
    public sealed class GetByIdRoleQueryValidator : AbstractValidator<GetByIdRoleQuery>
    {
        // 1. Localizer servisini constructor üzerinden içeri alıyoruz
        public GetByIdRoleQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Id)
                // 2. Hardcoded metinler yerine localizer'dan gelen anahtarları (Key) okuyoruz
                .NotNull().WithMessage(localizer["Messages.KeyRequired"])
                .NotEqual(Guid.Empty).WithMessage(localizer["Messages.ValueRequired"]);
        }
    }
}