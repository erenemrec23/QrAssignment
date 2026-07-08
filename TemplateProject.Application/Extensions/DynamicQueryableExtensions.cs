using QrAssignment.Application.DTOs.List;
using QrAssignment.Domain.Attributes;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace QrAssignment.Application.Extensions
{
    public static class DynamicQueryableExtensions
    {
        public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQueryDto dynamicQuery)
        {
            if (dynamicQuery.Filter != null)
            {
                var values = new List<object>();
                string whereQuery = Transform(dynamicQuery.Filter, values, typeof(T));

                if (!string.IsNullOrEmpty(whereQuery))
                {
                    query = query.Where(whereQuery, values.ToArray());
                }
            }

            if (dynamicQuery.Sort != null && dynamicQuery.Sort.Any())
            {
                string ordering = string.Join(",", dynamicQuery.Sort.Select(s => $"{s.Field} {s.Dir}"));
                query = query.OrderBy(ordering);
            }

            return query;
        }

        private static string Transform(DynamicQueryFilterDto filter, List<object> values, Type entityType)
        {

            string comparison = string.Empty;

            if (!string.IsNullOrEmpty(filter.Field) && !string.IsNullOrEmpty(filter.Operator))
            {
                var property = entityType.GetProperty(filter.Field);

                if (property == null)
                    throw new ArgumentException($"'{filter.Field}' alanı bulunamadı.");
                 
                bool isFilterable = property.GetCustomAttributes(typeof(FilterableAttribute), inherit: true).Any();
                if (!isFilterable)
                    throw new UnauthorizedAccessException($"'{filter.Field}' alanı üzerinden filtreleme yapılamaz.");

                int index = values.Count;
                comparison = GetComparison(filter.Operator, filter.Field, index, property.PropertyType);
                values.Add(ConvertValue(filter.Value, property.PropertyType));
            }

            if (filter.Filters != null && filter.Filters.Any())
            {
                string logic = filter.Logic ?? "and";
                var subFilters = new List<string>();

                foreach (var subFilter in filter.Filters)
                {
                    var subTransformed = Transform(subFilter, values, entityType);
                    if (!string.IsNullOrEmpty(subTransformed))
                    {
                        subFilters.Add(subTransformed);
                    }
                }

                if (subFilters.Any())
                {
                    string subFilterString = string.Join($" {logic} ", subFilters);

                    if (!string.IsNullOrEmpty(comparison))
                    {
                        return $"({comparison} {logic} ({subFilterString}))";
                    }

                    return $"({subFilterString})";
                }
            }

            return comparison;
        }

        private static string GetComparison(string op, string field, int index, Type propertyType)
        {
            bool isString = propertyType == typeof(string);
            string target = isString ? field : $"{field}.ToString()";

            return op.ToLower() switch
            {
                "eq" => $"{field} == @{index}",
                "neq" => $"{field} != @{index}",
                "gt" => $"{field} > @{index}",
                "gte" => $"{field} >= @{index}",
                "lt" => $"{field} < @{index}",
                "lte" => $"{field} <= @{index}",
                "startswith" => $"{target}.StartsWith(@{index})",
                "endswith" => $"{target}.EndsWith(@{index})",
                "contains" => $"{target}.Contains(@{index})",
                "doesnotcontain" => $"!{target}.Contains(@{index})",
                _ => $"{field} == @{index}"
            };
        }

        /// <summary>
        /// Gelen string değeri (filter.Value), hedef property'nin gerçek tipine çevirir.
        /// "eq"/"gt" gibi operatörlerde Dynamic LINQ'in tip uyuşmazlığından patlamaması için gereklidir.
        /// "contains" gibi string bazlı operatörlerde zaten target alan ToString()'e çevrildiği için
        /// value'yu string olarak bırakmak yeterlidir.
        /// </summary>
        private static object ConvertValue(string? value, Type propertyType)
        {
            if (value is null) return null!;

            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            try
            {
                if (underlyingType == typeof(string)) return value;
                if (underlyingType == typeof(Guid)) return Guid.Parse(value);
                if (underlyingType == typeof(bool)) return bool.Parse(value);
                if (underlyingType == typeof(DateTime)) return DateTime.Parse(value, CultureInfo.InvariantCulture);
                if (underlyingType == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);
                if (underlyingType.IsEnum) return Enum.Parse(underlyingType, value, ignoreCase: true);

                // int, long, decimal, double, float vb. IConvertible tipler
                return Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) when (ex is FormatException or OverflowException or InvalidCastException)
            {
                throw new ArgumentException(
                    $"'{value}' değeri '{propertyType.Name}' tipine dönüştürülemedi.", ex);
            }
        }
    }
}