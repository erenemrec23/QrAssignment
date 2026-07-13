using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class approle_audit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // RowVersion'ı alter etmek yerine drop + add
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AppRoles");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AppRoles",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[8]); // rowversion 8 byte'tır, new byte[0] değil new byte[8] daha doğru olur ama SQL Server zaten kendi değerini atayacak

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "AppRoles",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDate",
                table: "AppRoles",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "AppRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "AppRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "AppRoles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CreatedByUserId", table: "AppRoles");
            migrationBuilder.DropColumn(name: "ModifiedByUserId", table: "AppRoles");
            migrationBuilder.DropColumn(name: "RevNum", table: "AppRoles");

            migrationBuilder.DropColumn(name: "RowVersion", table: "AppRoles");
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AppRoles",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "AppRoles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "AppRoles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }
    }
}
