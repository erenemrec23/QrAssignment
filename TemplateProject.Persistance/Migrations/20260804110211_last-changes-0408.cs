using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class lastchanges0408 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_RoleId_PageId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_UserId_PageId",
                table: "PagePermissions");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "PagePermissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<short>(
                name: "MenuGroupId",
                table: "PagePermissions",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_MenuGroupId",
                table: "PagePermissions",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_RoleId_MenuGroupId",
                table: "PagePermissions",
                columns: new[] { "RoleId", "MenuGroupId" },
                unique: true,
                filter: "[RoleId] IS NOT NULL AND [MenuGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_RoleId_PageId",
                table: "PagePermissions",
                columns: new[] { "RoleId", "PageId" },
                unique: true,
                filter: "[RoleId] IS NOT NULL AND [PageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_UserId_MenuGroupId",
                table: "PagePermissions",
                columns: new[] { "UserId", "MenuGroupId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [MenuGroupId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_UserId_PageId",
                table: "PagePermissions",
                columns: new[] { "UserId", "PageId" },
                unique: true,
                filter: "[UserId] IS NOT NULL AND [PageId] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PagePermission_SingleTarget",
                table: "PagePermissions",
                sql: "([PageId] IS NOT NULL AND [MenuGroupId] IS NULL) OR ([PageId] IS NULL AND [MenuGroupId] IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermissions_MenuGroups_MenuGroupId",
                table: "PagePermissions",
                column: "MenuGroupId",
                principalTable: "MenuGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PagePermissions_MenuGroups_MenuGroupId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_MenuGroupId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_RoleId_MenuGroupId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_RoleId_PageId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_UserId_MenuGroupId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_UserId_PageId",
                table: "PagePermissions");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PagePermission_SingleTarget",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "MenuGroupId",
                table: "PagePermissions");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "PagePermissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_RoleId_PageId",
                table: "PagePermissions",
                columns: new[] { "RoleId", "PageId" },
                unique: true,
                filter: "[RoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_UserId_PageId",
                table: "PagePermissions",
                columns: new[] { "UserId", "PageId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
