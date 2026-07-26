using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedByUserId1",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedByUserId1",
                table: "Items",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedByUserId1",
                table: "Items",
                column: "CreatedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ModifiedByUserId1",
                table: "Items",
                column: "ModifiedByUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRegions_CreatedByUserId",
                table: "SystemRegions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemRegions_ModifiedByUserId",
                table: "SystemRegions",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QrLocations_CreatedByUserId",
                table: "QrLocations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_QrLocations_ModifiedByUserId",
                table: "QrLocations",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRefreshTokens_CreatedByUserId",
                table: "AppUserRefreshTokens",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRefreshTokens_ModifiedByUserId",
                table: "AppUserRefreshTokens",
                column: "ModifiedByUserId");

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
                name: "FK_Tenants_AppUsers_CreatedByUserId1",
                table: "Items",
                column: "CreatedByUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_AppUsers_ModifiedByUserId1",
                table: "Items",
                column: "ModifiedByUserId1",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_CreatedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRefreshTokens_AppUsers_ModifiedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUsers_CreatedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_QrLocations_AppUsers_ModifiedByUserId",
                table: "QrLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUsers_CreatedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemRegions_AppUsers_ModifiedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUsers_CreatedByUserId1",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_AppUsers_ModifiedByUserId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_CreatedByUserId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_ModifiedByUserId1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_SystemRegions_CreatedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropIndex(
                name: "IX_SystemRegions_ModifiedByUserId",
                table: "SystemRegions");

            migrationBuilder.DropIndex(
                name: "IX_QrLocations_CreatedByUserId",
                table: "QrLocations");

            migrationBuilder.DropIndex(
                name: "IX_QrLocations_ModifiedByUserId",
                table: "QrLocations");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRefreshTokens_CreatedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRefreshTokens_ModifiedByUserId",
                table: "AppUserRefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserId1",
                table: "Items");
        }
    }
}
