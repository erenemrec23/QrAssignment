using FluentValidation;
using Microsoft.Extensions.Localization;

public static class CustomValidationRules
{
    // Guid? için
    public static IRuleBuilderOptions<T, Guid?> MustBeValidId<T>(
        this IRuleBuilder<T, Guid?> ruleBuilder,
        IStringLocalizer localizer)
    {
        return ruleBuilder
            .NotNull().WithMessage(localizer["Messages.KeyRequired"])
            .NotEqual(Guid.Empty).WithMessage(localizer["InvalidKeyParameter"]);
    }

    // Guid için (nullable olmayan alanlar da varsa)
    public static IRuleBuilderOptions<T, Guid> MustBeValidId<T>(
        this IRuleBuilder<T, Guid> ruleBuilder,
        IStringLocalizer localizer)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(localizer["Messages.ValueRequired"]);
    }
}