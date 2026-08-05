using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Page_Permission_BaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "PagePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "PagePermissions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "IsPassived",
                table: "PagePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "PagePermissions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "PagePermissions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "PagePermissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PagePermissions",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_CreatedByUserId",
                table: "PagePermissions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PagePermissions_ModifiedByUserId",
                table: "PagePermissions",
                column: "ModifiedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermissions_AppUser_CreatedByUserId",
                table: "PagePermissions",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PagePermissions_AppUser_ModifiedByUserId",
                table: "PagePermissions",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PagePermissions_AppUser_CreatedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_PagePermissions_AppUser_ModifiedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_CreatedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropIndex(
                name: "IX_PagePermissions_ModifiedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "IsPassived",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "PagePermissions");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PagePermissions");
        }
    }
}
