namespace QrAssignment.Domain.Shared.Menu
{
    public enum AppMenuGroup : short
    {
        [MenuGroupDefinition(Icon = "bi-gear-wide-connected", Order = 1)]
        Admin = 1,
    }

    public enum AppPage
    {
        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Page_Users",
            TranslationKey = "Users", Icon = "bi-people", Route = "/users", Order = 1)]
        Users = 1,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Page_QrLocations",
            TranslationKey = "QrLocations", Icon = "bi-qr-code", Route = "/qr-locations", Order = 2)]
        QrLocations = 2,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Page_Tenants",
            TranslationKey = "Tenants", Icon = "bi-shop", Route = "/tenants", Order = 3)]
        Tenants = 3,

        [PageDefinition(Group = AppMenuGroup.Admin, PageKey = "Page_Roles",
            TranslationKey = "AppRoles", Icon = "bi-shield", Route = "/roles", Order = 4)]
        Roles = 4,

        // Menüde görünmeyen, sadece yetki kapsamı olan sayfa (AuthorizationRegistry'deki
        // Page_UserPermissions). Grup atanmadı → MenuGroupId null, ShowInMenu false.
        //[PageDefinition(PageKey = "Page_UserPermissions",
        //    TranslationKey = "UserPermissions", Order = 99, ShowInMenu = false)]
        //UserPermissions = 5,
    }
}