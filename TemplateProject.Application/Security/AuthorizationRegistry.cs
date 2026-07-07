using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Domain.Shared; // PagePermissions enum'ının olduğu yer
using System;
using System.Collections.Generic;

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
            { typeof(Features.Tenants.Queries.GetById.GetTenantByIdQuery), (AppPages.Tenants, PagePermissions.View) },
            { typeof(Features.Tenants.Queries.GetList.GetTenantListQuery), (AppPages.Tenants, PagePermissions.View) },
            { typeof(Features.Tenants.Queries.GetListExportExcel.GetTenantListExportExcelQuery), (AppPages.Tenants, PagePermissions.ExportExcel) },
            
            // Role İşlemleri
            { typeof(Features.Roles.Commands.Create.CreateRoleCommand), (AppPages.Roles, PagePermissions.Insert) },
            { typeof(Features.Roles.Commands.Update.UpdateRoleCommand), (AppPages.Roles, PagePermissions.Update) },
            { typeof(Features.Roles.Commands.Delete.DeleteRoleCommand), (AppPages.Roles, PagePermissions.Delete) },
            { typeof(Features.Roles.Queries.GetList.GetRoleListQuery), (AppPages.Roles, PagePermissions.View) },

            { typeof(Features.Users.Commands.Create.CreateUserCommand), (AppPages.Users, PagePermissions.Insert) },
            { typeof(Features.Users.Commands.Update.UpdateUserCommand), (AppPages.Users, PagePermissions.Update) },
            //{ typeof(Features.Users.Commands.Delete.DeleteUserCommand), (AppPages.Users, PagePermissions.Delete) }
            { typeof(Features.Users.Queries.GetList.GetUserListQuery), (AppPages.Users, PagePermissions.View) },
            { typeof(Features.Users.Queries.GetById.GetUserByIdQuery), (AppPages.Users, PagePermissions.View) },


            { typeof(Features.Permission.Queries.GetByUserId.GetUserPermissionByUserIdQuery), (AppPages.Users, PagePermissions.View) },
            { typeof(Features.Permission.Commands.Update.UpdateUserPermissionCommand), (AppPages.Users, PagePermissions.Update) },  


            { typeof(Features.QrLocations.Commands.Create.CreateQrLocationCommand), (AppPages.QrLocations, PagePermissions.Insert) },
            { typeof(Features.QrLocations.Commands.Update.UpdateQrLocationCommand), (AppPages.QrLocations, PagePermissions.Update) },
            { typeof(Features.QrLocations.Queries.GetList.GetQrLocationListQuery), (AppPages.QrLocations, PagePermissions.View) },
            { typeof(Features.QrLocations.Queries.GetById.GetQrLocationByIdQuery), (AppPages.QrLocations, PagePermissions.View) },
            //{ typeof(Features.QrLocations.Commands.Delete.DeleteQrLocationCommand), (AppPages.QrLocations, PagePermissions.Delete) },


        };

        // 2. SERBEST KOMUTLAR: Herkese açık işlemler (Login, Register vb.)
        public static readonly HashSet<Type> UnsecuredCommands = new()
        {
             { typeof(Features.AuthFeatures.Commands.Login.LoginCommand) }
        };
    }
}