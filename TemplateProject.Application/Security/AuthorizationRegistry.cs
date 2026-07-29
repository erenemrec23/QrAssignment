 
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Security
{
    // Sihirli metinleri önlemek için sabitler
    public static class AppPages
    {
        public const string Tenants = "Page_Tenants";
        public const string Users = "Page_Users";
        public const string Roles = "Page_Roles";
        public const string UserPermissions = "Page_UserPermissions";
        public const string QrLocations = "Page_QrLocations";
    }

    public static class AuthorizationRegistry
    {
        // 1. GÜVENLİ KOMUTLAR: Çalışması için yetki gerekenler
        public static readonly Dictionary<Type, (string PageName, PagePermissions Permission)> SecuredCommands = new()
        {
            // Tenant İşlemleri
            { typeof(Features.Tenants.Commands.Create.CreateTenantCommand), (AppPages.Tenants, PagePermissions.Insert) },
            { typeof(Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantCommand), (AppPages.Tenants, PagePermissions.ImportExcel) },
            { typeof(Features.Tenants.Commands.Update.UpdateTenantCommand), (AppPages.Tenants, PagePermissions.Update) },
            { typeof(Features.Tenants.Commands.Delete.DeleteTenantCommand), (AppPages.Tenants, PagePermissions.Delete) },
            { typeof(Features.Tenants.Commands.BulkDelete.BulkDeleteTenantCommand), (AppPages.Tenants, PagePermissions.Delete) },
            { typeof(Features.Tenants.Queries.GetById.GetByIdTenantQuery), (AppPages.Tenants, PagePermissions.View) },
            { typeof(Features.Tenants.Queries.GetList.GetListTenantQuery), (AppPages.Tenants, PagePermissions.View) },
            { typeof(Features.Tenants.Queries.GetPassiveById.GetPassivedByIdTenantQuery), (AppPages.Tenants, PagePermissions.ViewPassive) },
            { typeof(Features.Tenants.Queries.GetPassivedList.GetPassivedListTenantQuery), (AppPages.Tenants, PagePermissions.ViewPassive) },
            { typeof(Features.Tenants.Queries.GetListExportExcel.GetListTenantExportExcelQuery), (AppPages.Tenants, PagePermissions.ExportExcel) },
            { typeof(Features.Tenants.Commands.Excel.Validate.ValidateTenantExcelQuery), (AppPages.Tenants, PagePermissions.ImportExcel) },
            { typeof(Common.Excel.ValidateExcelQuery<Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantInputDto>),
  (AppPages.Tenants, PagePermissions.ImportExcel) },
            { typeof(Common.Excel.GetSampleExcelTemplateQuery<Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantInputDto>),
  (AppPages.Tenants, PagePermissions.ImportExcel) },
            { typeof(Features.Tenants.Commands.SetActive.SetActiveTenantCommand),
  (AppPages.Tenants, PagePermissions.SetActive) },

            



            { typeof(Features.Users.Commands.Create.CreateAppUserCommand), (AppPages.Users, PagePermissions.Insert) },
{ typeof(Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserCommand), (AppPages.Users, PagePermissions.ImportExcel) },
{ typeof(Features.Users.Commands.Update.UpdateAppUserCommand), (AppPages.Users, PagePermissions.Update) },
{ typeof(Features.Users.Commands.Delete.DeleteAppUserCommand), (AppPages.Users, PagePermissions.Delete) },
{ typeof(Features.Users.Commands.BulkDelete.BulkDeleteAppUserCommand), (AppPages.Users, PagePermissions.Delete) },
{ typeof(Features.Users.Queries.FormBase.GetById.GetByIdAppUserQuery), (AppPages.Users, PagePermissions.View) },
{ typeof(Features.Users.Queries.ListBase.GetList.GetListAppUserQuery), (AppPages.Users, PagePermissions.View) },
{ typeof(Features.Users.Queries.FormBase.GetPassivedById.GetPassivedByIdAppUserQuery), (AppPages.Users, PagePermissions.ViewPassive) },
{ typeof(Features.Users.Queries.ListBase.GetPassivedList.GetPassivedListAppUserQuery), (AppPages.Users, PagePermissions.ViewPassive) },
            { typeof(Features.Users.Queries.ListBase.GetListExportExcel.GetListAppUserExportExcelQuery), (AppPages.Users, PagePermissions.ExportExcel) },
{ typeof(Features.Users.Commands.Excel.Validate.ValidateAppUserExcelQuery), (AppPages.Users, PagePermissions.ImportExcel) },
{ typeof(Common.Excel.ValidateExcelQuery<Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserInputDto>),
    (AppPages.Users, PagePermissions.ImportExcel) },
{ typeof(Common.Excel.GetSampleExcelTemplateQuery<Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserInputDto>),
    (AppPages.Users, PagePermissions.ImportExcel) },
{ typeof(Features.Users.Commands.SetActive.SetActiveAppUserCommand),
    (AppPages.Users, PagePermissions.SetActive) },


            { typeof(Features.Permission.Queries.GetByUserId.GetUserPermissionByUserIdQuery), (AppPages.Users, PagePermissions.View) },
            { typeof(Features.Permission.Commands.Update.UpdateUserPermissionCommand), (AppPages.Users, PagePermissions.Update) },  


            { typeof(Features.QrLocations.Commands.Create.CreateQrLocationCommand), (AppPages.QrLocations, PagePermissions.Insert) },
            { typeof(Features.QrLocations.Commands.Update.UpdateQrLocationCommand), (AppPages.QrLocations, PagePermissions.Update) },
            { typeof(Features.QrLocations.Queries.GetList.GetQrLocationListQuery), (AppPages.QrLocations, PagePermissions.View) },
            { typeof(Features.QrLocations.Queries.GetById.GetQrLocationByIdQuery), (AppPages.QrLocations, PagePermissions.View) },
            //{ typeof(Features.QrLocations.Commands.Delete.DeleteQrLocationCommand), (AppPages.QrLocations, PagePermissions.Delete) },

                   
            { typeof(Features.Roles.Commands.Create.CreateAppRoleCommand), (AppPages.Roles, PagePermissions.Insert) },
{ typeof(Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleCommand), (AppPages.Roles, PagePermissions.ImportExcel) },
{ typeof(Features.Roles.Commands.Update.UpdateAppRoleCommand), (AppPages.Roles, PagePermissions.Update) },
{ typeof(Features.Roles.Commands.Delete.DeleteAppRoleCommand), (AppPages.Roles, PagePermissions.Delete) },
{ typeof(Features.Roles.Commands.BulkDelete.BulkDeleteAppRoleCommand), (AppPages.Roles, PagePermissions.Delete) },
{ typeof(Features.Roles.Queries.FormBase.GetById.GetByIdRoleQuery), (AppPages.Roles, PagePermissions.View) },
{ typeof(Features.Roles.Queries.ListBase.GetList.GetListAppRoleQuery), (AppPages.Roles, PagePermissions.View) },
{ typeof(Features.Roles.Queries.FormBase.GetPassivedById.GetPassivedByIdAppRoleQuery), (AppPages.Roles, PagePermissions.ViewPassive) },
{ typeof(Features.Roles.Queries.ListBase.GetPassivedList.GetPassivedListAppRoleQuery), (AppPages.Roles, PagePermissions.ViewPassive) },
{ typeof(Features.Roles.Queries.ListBase.GetListExportExcel.GetListAppRoleExportExcelQuery), (AppPages.Roles, PagePermissions.ExportExcel) },
{ typeof(Features.Roles.Commands.Excel.Validate.ValidateAppRoleExcelQuery), (AppPages.Roles, PagePermissions.ImportExcel) },
{ typeof(Common.Excel.ValidateExcelQuery<Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleInputDto>),
    (AppPages.Roles, PagePermissions.ImportExcel) },
{ typeof(Common.Excel.GetSampleExcelTemplateQuery<Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleInputDto>),
    (AppPages.Roles, PagePermissions.ImportExcel) },
            { typeof(Features.Users.Queries.LookUp.GetLookupList.GetLookUpListAppUserQuery), (AppPages.Roles, PagePermissions.View) },
            { typeof(Features.Roles.Queries.LookUp.GetAssignedUserList.GetRoleAssignedUserListQuery), (AppPages.Roles, PagePermissions.View) },
            { typeof(Features.Roles.Queries.GetAssignedPermissionList.GetAssignedPermissionListQuery), (AppPages.Roles, PagePermissions.View) },
            { typeof(Features.Roles.Commands.SetActive.SetActiveAppRoleCommand), (AppPages.Roles, PagePermissions.SetActive) },
            

        };

        // 2. SERBEST KOMUTLAR: Herkese açık işlemler (Login, Register vb.)
        public static readonly HashSet<Type> UnsecuredCommands = new()
        {
             { typeof(Features.AuthFeatures.Commands.Login.LoginCommand) }, 
            
        };
    }
}