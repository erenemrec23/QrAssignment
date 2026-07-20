using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Domain.Shared
{
    [Flags] // Bu attribute çok kritiktir!
    public enum PagePermissions : int
    {
        None = 0,
        View = 1,          
        Insert = 2,         
        Update = 4,        
        SetPassive = 8,
        SetActive = 16,
        Delete = 32,
        ExportExcel = 64,  
        ImportExcel = 128,

        // Sık kullanılan kombinasyonlar (Opsiyonel)
        ViewAndInsert = View | Insert,  
        All = View | Insert | Update | Delete | SetPassive | SetActive | ExportExcel | ImportExcel 
    }
}
