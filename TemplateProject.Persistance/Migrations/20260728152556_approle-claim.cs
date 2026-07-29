using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TemplateProject.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class approleclaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
             

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppRoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppRoles_CreatedByUserId",
                table: "AppRoles",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoles_ModifiedByUserId",
                table: "AppRoles",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AppRoleClaims",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoles_AppUser_CreatedByUserId",
                table: "AppRoles",
                column: "CreatedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoles_AppUser_ModifiedByUserId",
                table: "AppRoles",
                column: "ModifiedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
             
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoles_AppUser_CreatedByUserId",
                table: "AppRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppRoles_AppUser_ModifiedByUserId",
                table: "AppRoles");
             

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropIndex(
                name: "IX_AppRoles_CreatedByUserId",
                table: "AppRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppRoles_ModifiedByUserId",
                table: "AppRoles");
             
             
        }
    }
}
