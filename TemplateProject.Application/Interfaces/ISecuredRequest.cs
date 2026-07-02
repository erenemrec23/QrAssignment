using QrAssignment.Domain.Shared; // PagePermissions enum'ının olduğu yer

namespace QrAssignment.Application.Interfaces
{
    public interface ISecuredRequest
    {
        // Yetkinin kontrol edileceği sayfanın/modülün adı (Örn: "Page_Tenants")
        string PageName { get; }

        // Bu işlem için gereken bitwise yetki değeri (Örn: PagePermissions.ExportExcel)
        PagePermissions RequiredPermission { get; }
    }
    public interface INotSecuredRequest
    { 
    }
}