using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class eksik_kolonlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RevNum",
                table: "AppUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "AppRoles",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDate",
                table: "AppRoles",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AppRoles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId",
                table: "AppRoles");

            migrationBuilder.DropColumn(
                name: "RevNum",
                table: "AppRoles");

            migrationBuilder.AlterColumn<byte[]>(
                name: "RowVersion",
                table: "AppRoles",
                type: "rowversion",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "rowversion",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "AppRoles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AppRoles",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

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
