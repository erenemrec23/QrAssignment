using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class newrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations");

            migrationBuilder.DropIndex(
                name: "IX_QrLocations_ParentLocationId",
                table: "QrLocations");

            migrationBuilder.DropColumn(
                name: "ParentLocationId",
                table: "QrLocations");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QrLocations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "QrLocations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MenuGroups",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PageKey = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ShowInMenu = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MenuGroupId = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_MenuGroups_MenuGroupId",
                        column: x => x.MenuGroupId,
                        principalTable: "MenuGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrLocations_Name",
                table: "QrLocations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_CreatedByUserId",
                table: "AppUser",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUser_ModifiedByUserId",
                table: "AppUser",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_MenuGroupId",
                table: "Pages",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_PageKey",
                table: "Pages",
                column: "PageKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_AppUser_CreatedByUserId",
                table: "AppUser",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUser_AppUser_ModifiedByUserId",
                table: "AppUser",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_AppUser_CreatedByUserId",
                table: "AppUser");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUser_AppUser_ModifiedByUserId",
                table: "AppUser");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "MenuGroups");

            migrationBuilder.DropIndex(
                name: "IX_QrLocations_Name",
                table: "QrLocations");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_CreatedByUserId",
                table: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_AppUser_ModifiedByUserId",
                table: "AppUser");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QrLocations",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "QrLocations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentLocationId",
                table: "QrLocations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrLocations_ParentLocationId",
                table: "QrLocations",
                column: "ParentLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_QrLocations_QrLocations_ParentLocationId",
                table: "QrLocations",
                column: "ParentLocationId",
                principalTable: "QrLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
