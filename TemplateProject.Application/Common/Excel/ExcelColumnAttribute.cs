namespace QrAssignment.Application.Common.Excel
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ExcelColumnAttribute : Attribute
    {
        // Artık literal kolon adı değil, localization key
        public string LocalizationKey { get; }
        public ExcelColumnAttribute(string localizationKey) => LocalizationKey = localizationKey;
    }

    // Ortak taban: her validasyon attribute'u ister literal ister key ile mesaj verebilsin
    public abstract class ExcelValidationAttributeBase : Attribute
    {
        public string? ErrorMessageKey { get; set; }  // örn: "Excel.Error.CodeRequired"
        public string? ErrorMessage { get; set; }     // literal fallback (opsiyonel, key yoksa kullanılır)
    }

    public sealed class ExcelRequiredAttribute : ExcelValidationAttributeBase { }

    public sealed class ExcelMaxLengthAttribute : ExcelValidationAttributeBase
    {
        public int Length { get; }
        public ExcelMaxLengthAttribute(int length) => Length = length;
    }

    public sealed class ExcelRangeAttribute : ExcelValidationAttributeBase
    {
        public double Min { get; set; } = double.MinValue;
        public double Max { get; set; } = double.MaxValue;
    }

    public sealed class ExcelRegexAttribute : ExcelValidationAttributeBase
    {
        public string Pattern { get; }
        public ExcelRegexAttribute(string pattern) => Pattern = pattern;
    }

    public sealed class ExcelUniqueInFileAttribute : ExcelValidationAttributeBase { }
}