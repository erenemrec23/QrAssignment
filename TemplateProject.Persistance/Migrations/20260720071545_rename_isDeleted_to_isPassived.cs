using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class rename_isDeleted_to_isPassived : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Tenants",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "SystemRegions",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "QrLocations",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AppUsers",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AppUserRole",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AppUserRefreshTokens",
                newName: "IsPassived");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AppRoles",
                newName: "IsPassived");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "Tenants",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "SystemRegions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "QrLocations",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "AppUsers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "AppUserRole",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "AppUserRefreshTokens",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "IsPassived",
                table: "AppRoles",
                newName: "IsDeleted");
        }
    }
}
