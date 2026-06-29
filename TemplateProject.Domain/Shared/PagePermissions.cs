using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Domain.Shared
{
    [Flags] // Bu attribute çok kritiktir!
    public enum PagePermissions : int
    {
        None = 0,
        View = 1,          // 2^0
        Insert = 2,        // 2^1
        Update = 4,        // 2^2
        Delete = 8,        // 2^3
        ExportExcel = 16,  // 2^4
        ImportExcel = 32,  // 2^5

        // Sık kullanılan kombinasyonlar (Opsiyonel)
        ViewAndInsert = View | Insert, // 3
        All = View | Insert | Update | Delete | ExportExcel | ImportExcel // 63
    }
}
