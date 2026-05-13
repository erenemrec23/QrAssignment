using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class added_auditEntity_ColumnValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColumnValues",
                table: "SystemAuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnValues",
                table: "SystemAuditLogs");
        }
    }
}
