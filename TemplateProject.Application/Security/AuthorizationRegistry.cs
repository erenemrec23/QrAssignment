using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetList;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetPassivedList;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetById;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetList;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetPassivedList;
using QrAssignment.Domain.Shared.PagePermission;
using RolesBulkExcelDto = QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleInputDto;
// Alias tanımlamaları ile tip isimleri ve okunabilirlik sadeleştirildi
using TenantsBulkExcelDto = QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantInputDto;
using UsersBulkExcelDto = QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserInputDto;

namespace QrAssignment.Application.Security
{
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
        public static readonly Dictionary<Type, (string PageName, PagePermissions Permission)> SecuredCommands;
        public static readonly HashSet<Type> UnsecuredCommands;

        static AuthorizationRegistry()
        {
            var registry = new Dictionary<Type, (string PageName, PagePermissions Permission)>();

            // =========================================================================
            // TENANTS (Page_Tenants)
            // =========================================================================
            Register(registry, AppPages.Tenants, PagePermissions.Insert,
                typeof(Features.Tenants.Commands.Create.CreateTenantCommand));

            Register(registry, AppPages.Tenants, PagePermissions.Update,
                typeof(Features.Tenants.Commands.Update.UpdateTenantCommand),
                typeof(Features.Tenants.Commands.SetActive.SetActiveTenantCommand));

            Register(registry, AppPages.Tenants, PagePermissions.Delete,
                typeof(Features.Tenants.Commands.Delete.DeleteTenantCommand),
                typeof(Features.Tenants.Commands.BulkDelete.BulkDeleteTenantCommand));

            Register(registry, AppPages.Tenants, PagePermissions.View,
                typeof(GetByIdTenantQuery),
                typeof(GetListTenantQuery));

            Register(registry, AppPages.Tenants, PagePermissions.SetPassive,
                typeof(Features.Tenants.Commands.SetPassive.SetPassiveTenantCommand),
                typeof(Features.Tenants.Commands.BulkSetPassive.BulkSetPassiveTenantCommand));

            Register(registry, AppPages.Tenants, PagePermissions.SetActive,
                typeof(Features.Tenants.Commands.SetActive.SetActiveTenantCommand),
                typeof(Features.Tenants.Commands.BulkSetActive.BulkSetActiveTenantCommand));

            Register(registry, AppPages.Tenants, PagePermissions.ViewPassive,
                typeof(GetPassivedByIdTenantQuery),
                typeof(GetPassivedListTenantQuery));

            Register(registry, AppPages.Tenants, PagePermissions.ExportExcel,
                typeof(GetListTenantExportExcelQuery));

            Register(registry, AppPages.Tenants, PagePermissions.ImportExcel,
                typeof(Features.Tenants.Commands.Excel.BulkCreate.BulkCreateTenantCommand),
                typeof(Features.Tenants.Commands.Excel.Validate.ValidateTenantExcelQuery),
                typeof(ValidateExcelQuery<TenantsBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<TenantsBulkExcelDto>));


            // =========================================================================
            // USERS (Page_Users)
            // =========================================================================
            Register(registry, AppPages.Users, PagePermissions.Insert,
                typeof(Features.Users.Commands.Create.CreateAppUserCommand));

            Register(registry, AppPages.Users, PagePermissions.Update,
                typeof(Features.Users.Commands.Update.UpdateAppUserCommand),
                typeof(Features.Users.Commands.SetActive.SetActiveAppUserCommand),
                typeof(Features.Permission.Commands.Update.UpdateUserPermissionCommand));

            Register(registry, AppPages.Users, PagePermissions.Delete,
                typeof(Features.Users.Commands.Delete.DeleteAppUserCommand),
                typeof(Features.Users.Commands.BulkDelete.BulkDeleteAppUserCommand));

            Register(registry, AppPages.Users, PagePermissions.View,
                typeof(Features.Users.Queries.FormBase.GetById.GetByIdAppUserQuery),
                typeof(Features.Users.Queries.ListBase.GetList.GetListAppUserQuery),
                typeof(Features.Users.Queries.LookUp.GetLookupList.GetLookUpListAppUserQuery),
                typeof(Features.Permission.Queries.GetByUserId.GetUserPermissionByUserIdQuery));

            Register(registry, AppPages.Users, PagePermissions.SetPassive,
                typeof(Features.Users.Commands.SetPassive.SetPassiveAppUserCommand),
                typeof(Features.Users.Commands.BulkSetPassive.BulkSetPassiveAppUserCommand));

            Register(registry, AppPages.Users, PagePermissions.SetActive,
                typeof(Features.Users.Commands.SetActive.SetActiveAppUserCommand),
                typeof(Features.Users.Commands.BulkSetActive.BulkSetActiveAppUserCommand));

            Register(registry, AppPages.Users, PagePermissions.ViewPassive,
                typeof(Features.Users.Queries.FormBase.GetPassivedById.GetPassivedByIdAppUserQuery),
                typeof(Features.Users.Queries.ListBase.GetPassivedList.GetPassivedListAppUserQuery));

            Register(registry, AppPages.Users, PagePermissions.ExportExcel,
                typeof(Features.Users.Queries.ListBase.GetListExportExcel.GetListAppUserExportExcelQuery));

            Register(registry, AppPages.Users, PagePermissions.ImportExcel,
                typeof(Features.Users.Commands.Excel.BulkCreate.BulkCreateAppUserCommand),
                typeof(Features.Users.Commands.Excel.Validate.ValidateAppUserExcelQuery),
                typeof(ValidateExcelQuery<UsersBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<UsersBulkExcelDto>));


            // =========================================================================
            // ROLES (Page_Roles)
            // =========================================================================
            Register(registry, AppPages.Roles, PagePermissions.Insert,
                typeof(Features.Roles.Commands.Create.CreateAppRoleCommand));

            Register(registry, AppPages.Roles, PagePermissions.Update,
                typeof(Features.Roles.Commands.Update.UpdateAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetActive.BulkSetActiveAppRoleCommand));

            Register(registry, AppPages.Roles, PagePermissions.Delete,
                typeof(Features.Roles.Commands.Delete.DeleteAppRoleCommand),
                typeof(Features.Roles.Commands.BulkDelete.BulkDeleteAppRoleCommand));

            Register(registry, AppPages.Roles, PagePermissions.View,
                typeof(Features.Roles.Queries.FormBase.GetById.GetByIdRoleQuery),
                typeof(Features.Roles.Queries.ListBase.GetList.GetListAppRoleQuery),
                typeof(Features.Roles.Queries.LookUp.GetAssignedUserList.GetRoleAssignedUserListQuery),
                typeof(Features.Roles.Queries.GetAssignedPermissionList.GetRoleAssignedPermissionListQuery));

            Register(registry, AppPages.Roles, PagePermissions.SetPassive,
                typeof(Features.Roles.Commands.SetPassive.SetPassiveAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetPassive.BulkSetPassiveAppRoleCommand));

            Register(registry, AppPages.Roles, PagePermissions.SetActive,
                typeof(Features.Roles.Commands.SetActive.SetActiveAppRoleCommand),
                typeof(Features.Roles.Commands.BulkSetActive.BulkSetActiveAppRoleCommand));

            Register(registry, AppPages.Roles, PagePermissions.ViewPassive,
                typeof(Features.Roles.Queries.FormBase.GetPassivedById.GetPassivedByIdAppRoleQuery),
                typeof(Features.Roles.Queries.ListBase.GetPassivedList.GetPassivedListAppRoleQuery));

            Register(registry, AppPages.Roles, PagePermissions.ExportExcel,
                typeof(Features.Roles.Queries.ListBase.GetListExportExcel.GetListAppRoleExportExcelQuery));

            Register(registry, AppPages.Roles, PagePermissions.ImportExcel,
                typeof(Features.Roles.Commands.Excel.BulkCreate.BulkCreateAppRoleCommand),
                typeof(Features.Roles.Commands.Excel.Validate.ValidateAppRoleExcelQuery),
                typeof(ValidateExcelQuery<RolesBulkExcelDto>),
                typeof(GetSampleExcelTemplateQuery<RolesBulkExcelDto>));


            // =========================================================================
            // QR LOCATIONS (Page_QrLocations)
            // =========================================================================
            Register(registry, AppPages.QrLocations, PagePermissions.Insert,
    typeof(Features.QrLocations.Commands.Create.CreateQrLocationCommand));

            Register(registry, AppPages.QrLocations, PagePermissions.Update,
                typeof(Features.QrLocations.Commands.Update.UpdateQrLocationCommand),
                typeof(Features.QrLocations.Commands.SetActive.SetActiveQrLocationCommand));

            Register(registry, AppPages.QrLocations, PagePermissions.Delete,
                typeof(Features.QrLocations.Commands.Delete.DeleteQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkDelete.BulkDeleteQrLocationCommand));

            Register(registry, AppPages.QrLocations, PagePermissions.View,
                typeof(GetByIdQrLocationQuery),
                typeof(GetListQrLocationQuery));

            Register(registry, AppPages.QrLocations, PagePermissions.SetPassive,
                typeof(Features.QrLocations.Commands.SetPassive.SetPassiveQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkSetPassive.BulkSetPassiveQrLocationCommand));

            Register(registry, AppPages.QrLocations, PagePermissions.SetActive,
                typeof(Features.QrLocations.Commands.SetActive.SetActiveQrLocationCommand),
                typeof(Features.QrLocations.Commands.BulkSetActive.BulkSetActiveQrLocationCommand));

            Register(registry, AppPages.QrLocations, PagePermissions.ViewPassive,
                typeof(GetPassivedByIdQrLocationQuery),
                typeof(GetPassivedListQrLocationQuery));

            Register(registry, AppPages.QrLocations, PagePermissions.ExportExcel,
                typeof(GetListQrLocationExportExcelQuery));

            Register(registry, AppPages.QrLocations, PagePermissions.ImportExcel,
                typeof(Features.QrLocations.Commands.Excel.BulkCreate.BulkCreateQrLocationCommand),
                typeof(Features.QrLocations.Commands.Excel.Validate.ValidateQrLocationExcelQuery),
                typeof(ValidateExcelQuery<BulkCreateQrLocationInputDto>),
                typeof(GetSampleExcelTemplateQuery<BulkCreateQrLocationInputDto>));

            SecuredCommands = registry;

            // =========================================================================
            // UNSECURED COMMANDS
            // =========================================================================
            UnsecuredCommands = new HashSet<Type>
            {
                typeof(Features.AuthFeatures.Commands.Login.LoginCommand),
                typeof(Features.AuthFeatures.Commands.ForgotPassword.ForgotPasswordCommand),
                typeof(Features.AuthFeatures.Commands.ResetPassword.ResetPasswordCommand),
                typeof(GetSystemModulesQuery),
            };
        }
        
        private static void Register(
            Dictionary<Type, (string PageName, PagePermissions Permission)> registry,
            string pageName,
            PagePermissions permission,
            params Type[] commandTypes)
        {
            foreach (var type in commandTypes)
            {
                registry[type] = (pageName, permission);
            }
        }
    }
}