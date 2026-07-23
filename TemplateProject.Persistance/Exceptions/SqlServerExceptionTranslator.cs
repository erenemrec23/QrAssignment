// QrAssignment.Persistance/Exceptions/SqlServerExceptionTranslator.cs
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace QrAssignment.Persistance.Exceptions;

internal sealed partial class SqlServerExceptionTranslator : IDbExceptionTranslator
{
    private const int UniqueIndexViolation = 2601;
    private const int UniqueConstraintViolation = 2627;
    private const int ForeignKeyViolation = 547;

    // Index/constraint adı → kullanıcıya gösterilecek alan adı
    private static readonly Dictionary<string, string> FieldNames =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["IX_Tenants_Name"] = "Firma Adı",
        };

    public bool TryTranslate(Exception exception, out Exception translated)
    {
        translated = exception;

        if (exception is not DbUpdateException { InnerException: SqlException sql })
            return false;

        switch (sql.Number)
        {
            case UniqueIndexViolation:
            case UniqueConstraintViolation:
                translated = BuildDuplicate(sql, exception);
                return true;

            case ForeignKeyViolation:
                translated = new BusinessException(
                    "Bu kayıt başka kayıtlarla ilişkili olduğu için işlem tamamlanamadı.");
                return true;

            default:
                return false;
        }
    }

    private static DuplicateEntityException BuildDuplicate(SqlException sql, Exception inner)
    {
        var name = NameRegex().Match(sql.Message).Groups["name"].Value;
        var value = ValueRegex().Match(sql.Message).Groups["value"].Value;
        var field = FieldNames.TryGetValue(name, out var f) ? f : name;

        return new DuplicateEntityException(
            $"'{value}' değeri zaten kayıtlı. ({field})", field, value);
    }

    [GeneratedRegex(@"(?:index|constraint) '(?<name>[^']+)'")]
    private static partial Regex NameRegex();

    [GeneratedRegex(@"duplicate key value is \((?<value>.*?)\)")]
    private static partial Regex ValueRegex();
}