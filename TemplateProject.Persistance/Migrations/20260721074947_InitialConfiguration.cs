using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class InitialConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_AppUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_CreatedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_ModifiedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRole_AppRoles_AppRoleId",
                table: "AppUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRole_AppUsers_AppUserId",
                table: "AppUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUsers_CreatedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUsers_ModifiedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUsers_CreatedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUsers_ModifiedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_SystemRegions_ParentRegionId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUsers_CreatedByUserId1",
                table: "Tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUsers_ModifiedByUserId1",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CreatedByUserId1",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_ModifiedByUserId1",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRole",
                table: "AppUserRole");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId1",
                table: "Tenants");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUserRole",
                newName: "AppUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRole_AppUserId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRole_AppRoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_AppRoleId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenants",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "Tenants",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SystemRegions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "SystemRegions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "SystemRegions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemRegions",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QrLocations",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "QrLocations",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AppUserRefreshTokens",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "AppUserRefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "QrApplicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TCKN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsPassived = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    RevNum = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrApplicants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrApplicants_AppUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QrApplicants_AppUser_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QrApplicants_SystemRegions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "SystemRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedByUserId",
                table: "Tenants",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ModifiedByUserId",
                table: "Tenants",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Name",
                table: "Tenants",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrApplicants_CreatedByUserId",
                table: "QrApplicants",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QrApplicants_ModifiedByUserId",
                table: "QrApplicants",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QrApplicants_RegionId",
                table: "QrApplicants",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserClaims_AppUser_UserId",
                table: "AppUserClaims",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_AppUserId",
                table: "AppUserRefreshTokens",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_CreatedByUserId",
                table: "AppUserRefreshTokens",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_ModifiedByUserId",
                table: "AppUserRefreshTokens",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppRoles_AppRoleId",
                table: "AppUserRoles",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppUser_AppUserId",
                table: "AppUserRoles",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_AppUser_CreatedByUserId",
                table: "QrLocations",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_AppUser_ModifiedByUserId",
                table: "QrLocations",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations",
                column: "ParentLocationId",
                principalTable: "QrLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_AppUser_CreatedByUserId",
                table: "SystemRegions",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_AppUser_ModifiedByUserId",
                table: "SystemRegions",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_SystemRegions_ParentRegionId",
                table: "SystemRegions",
                column: "ParentRegionId",
                principalTable: "SystemRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AppUser_CreatedByUserId",
                table: "Tenants",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AppUser_ModifiedByUserId",
                table: "Tenants",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserClaims_AppUser_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_AppUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_CreatedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUser_ModifiedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppRoles_AppRoleId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppUser_AppUserId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUser_CreatedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUser_ModifiedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUser_CreatedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUser_ModifiedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_SystemRegions_ParentRegionId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUser_CreatedByUserId",
                table: "Tenants");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUser_ModifiedByUserId",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "QrApplicants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CreatedByUserId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_ModifiedByUserId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_Name",
                table: "Tenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUserRoles",
                newName: "AppUserRole");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AppUsers");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_AppUserId",
                table: "AppUserRole",
                newName: "IX_AppUserRole_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_AppRoleId",
                table: "AppUserRole",
                newName: "IX_AppUserRole_AppRoleId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "Tenants",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId1",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId1",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SystemRegions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "SystemRegions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "SystemRegions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "SystemRegions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QrLocations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "QrLocations",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "AppUserRefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<bool>(
                name: "IsPassived",
                table: "AppUserRefreshTokens",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRole",
                table: "AppUserRole",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedByUserId1",
                table: "Tenants",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ModifiedByUserId1",
                table: "Tenants",
                column: "ModifiedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_AppUserId",
                table: "AppUserRefreshTokens",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_CreatedByUserId",
                table: "AppUserRefreshTokens",
                column: "CreatedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_ModifiedByUserId",
                table: "AppUserRefreshTokens",
                column: "ModifiedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRole_AppRoles_AppRoleId",
                table: "AppUserRole",
                column: "AppRoleId",
                principalTable: "AppRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRole_AppUsers_AppUserId",
                table: "AppUserRole",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_AppUsers_CreatedByUserId",
                table: "QrLocations",
                column: "CreatedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_AppUsers_ModifiedByUserId",
                table: "QrLocations",
                column: "ModifiedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations",
                column: "ParentLocationId",
                principalTable: "QrLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_AppUsers_CreatedByUserId",
                table: "SystemRegions",
                column: "CreatedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_AppUsers_ModifiedByUserId",
                table: "SystemRegions",
                column: "ModifiedByUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemRegions_SystemRegions_ParentRegionId",
                table: "SystemRegions",
                column: "ParentRegionId",
                principalTable: "SystemRegions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AppUsers_CreatedByUserId1",
                table: "Tenants",
                column: "CreatedByUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AppUsers_ModifiedByUserId1",
                table: "Tenants",
                column: "ModifiedByUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
