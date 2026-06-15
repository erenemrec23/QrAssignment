namespace QrAssignment.Application.DTOs.List
{
    // 1. En Üst Kapsayıcı Sınıf
    public class PageRequestBaseDto
    {
        public int PageIndex { get; set; } = 0; // Sayfa numarası (0 veya 1'den başlatmak sana kalmış)
        public int PageSize { get; set; } = 10; // Sayfa başına kayıt
        public DynamicQueryDto? DynamicFilterAndSort { get; set; } // Sıralama ve Filtreleme detayları
    }

    // 2. Dinamik Sorgu Gövdesi
    public class DynamicQueryDto
    {
        public IEnumerable<DynamicQuerySortDto>? Sort { get; set; }
        public DynamicQueryFilterDto? Filter { get; set; }
    }

    // 3. Kolon Bazlı Sıralama (Birden fazla kolona göre sıralama yapılabilir)
    public class DynamicQuerySortDto
    {
        public string Field { get; set; } // Kolon adı (Örn: "Name", "CreatedDate")
        public string Dir { get; set; }   // Yön: "asc" veya "desc"

        public DynamicQuerySortDto() { }
        public DynamicQuerySortDto(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }
    }

    // 4. Kolon Bazlı Dinamik Filtre (Kendi içinde Recursive/Özyineli çalışır)
    public class DynamicQueryFilterDto
    {
        public string Field { get; set; }      // Filtrelenecek kolon (Örn: "Name")
        public string Operator { get; set; }   // eq, contains, startswith, gt (büyük), lt (küçük) vb.
        public string? Value { get; set; }     // Aranan değer
        public string? Logic { get; set; }     // "and" veya "or" (Alt filtreleri bağlamak için)

        // İşin sırrı burası: Filtreleri iç içe gruplamanı sağlar!
        public IEnumerable<DynamicQueryFilterDto>? Filters { get; set; }

        public DynamicQueryFilterDto() { }
    }

    public class Paginate<T>
    {
        public IList<T> Items { get; set; } // Asıl verilerin olduğu liste 

        public int Index { get; set; }  // Mevcut sayfa numarası

        private int? _pageSize;
        public int? PageSize
        {
            get { return _pageSize ?? 10; }
            set { _pageSize = value; }
        }

        public int TotalFilteredItemCount { get; set; }  // Filtre sonrası kalan kayıt sayısı
        public int TotalItemCount { get; set; }          // Veritabanındaki filtresiz toplam kayıt sayısı

        // DÜZELTİLDİ: Filtrelenmiş kayıt sayısını, sayfa boyutuna bölüyoruz
        public int TotalPages => (int)Math.Ceiling(TotalFilteredItemCount / (double)(PageSize ?? 10));

        // Frontend'deki "Önceki/Sonraki" butonlarını disable/enable yapmak için 
        public bool HasPrevious => Index > 0;
        public bool HasNext => Index + 1 < TotalPages;

        public Paginate()
        {
            Items = Array.Empty<T>();
        }
    }
}
