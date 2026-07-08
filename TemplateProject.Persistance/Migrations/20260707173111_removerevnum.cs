using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class removerevnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "SystemRegions");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "QrLocations");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "AppUserRefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "Tenants",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "SystemRegions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "QrLocations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "AppUserRefreshTokens",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
