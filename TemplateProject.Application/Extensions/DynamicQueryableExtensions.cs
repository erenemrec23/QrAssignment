using System.Linq.Dynamic.Core;
using QrAssignment.Application.DTOs.List;

namespace QrAssignment.Application.Extensions
{
    public static class DynamicQueryableExtensions
    {
        public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQueryDto dynamicQuery)
        {
            // 1. FİLTRELEME (Where) İŞLEMİ
            if (dynamicQuery.Filter != null)
            {
                // Parametreleri (örn: "Eren", 30) tutacağımız liste
                var values = new List<object>();

                // Recursive metot ile SQL where string'ini oluşturuyoruz
                string whereQuery = Transform(dynamicQuery.Filter, values);

                if (!string.IsNullOrEmpty(whereQuery))
                {
                    // System.Linq.Dynamic.Core'un string tabanlı Where metodu
                    // Örn: query.Where("Name.Contains(@0) and Age > @1", "Eren", 30)
                    query = query.Where(whereQuery, values.ToArray());
                }
            }

            // 2. SIRALAMA (OrderBy) İŞLEMİ
            if (dynamicQuery.Sort != null && dynamicQuery.Sort.Any())
            {
                // "Name asc, CreatedDate desc" formatına çeviriyoruz
                string ordering = string.Join(",", dynamicQuery.Sort.Select(s => $"{s.Field} {s.Dir}"));

                // System.Linq.Dynamic.Core'un string tabanlı OrderBy metodu
                query = query.OrderBy(ordering);
            }

            return query;
        }

        // --- YARDIMCI METOTLAR ---

        // Recursive (Özyineli) Filtre Çevirici
        private static string Transform(DynamicQueryFilterDto filter, List<object> values)
        {
            string comparison = string.Empty;

            // Eğer geçerli bir alan ve operatör varsa, bu bir uç (leaf) filtredir
            if (!string.IsNullOrEmpty(filter.Field) && !string.IsNullOrEmpty(filter.Operator))
            {
                int index = values.Count; // @0, @1, @2 için sayaç
                comparison = GetComparison(filter.Operator, filter.Field, index);

                // Değeri (@0'ın karşılığını) listeye ekliyoruz
                values.Add(filter.Value!);
            }

            // Alt filtreler (Filters dizisi) varsa, kendi içinde tekrar (recursive) dön
            if (filter.Filters != null && filter.Filters.Any())
            {
                string logic = filter.Logic ?? "and"; // Logic yoksa varsayılan AND kabul et
                var subFilters = new List<string>();

                foreach (var subFilter in filter.Filters)
                {
                    var subTransformed = Transform(subFilter, values);
                    if (!string.IsNullOrEmpty(subTransformed))
                    {
                        subFilters.Add(subTransformed);
                    }
                }

                if (subFilters.Any())
                {
                    string subFilterString = string.Join($" {logic} ", subFilters);

                    // Hem kendi üst filtresi hem de alt filtreler varsa ikisini parantezle bağla
                    if (!string.IsNullOrEmpty(comparison))
                    {
                        return $"({comparison} {logic} ({subFilterString}))";
                    }

                    // Sadece grup filtresiyse (Örn: Sadece AND ve altındakiler varsa)
                    return $"({subFilterString})";
                }
            }

            return comparison;
        }

        // Operatörleri System.Linq.Dynamic.Core formatına çeviren metot
        private static string GetComparison(string op, string field, int index)
        {
            return op.ToLower() switch
            {
                "eq" => $"{field} == @{index}",
                "neq" => $"{field} != @{index}",
                "gt" => $"{field} > @{index}",
                "gte" => $"{field} >= @{index}",
                "lt" => $"{field} < @{index}",
                "lte" => $"{field} <= @{index}",
                "startswith" => $"{field}.StartsWith(@{index})",
                "endswith" => $"{field}.EndsWith(@{index})",
                "contains" => $"{field}.Contains(@{index})",
                "doesnotcontain" => $"!{field}.Contains(@{index})",
                _ => $"{field} == @{index}" // Bilinmeyen bir şey gelirse eşittir kabul et
            };
        }
    }
}