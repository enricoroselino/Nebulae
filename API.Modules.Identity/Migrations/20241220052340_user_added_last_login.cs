using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class user_added_last_login : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                schema: "Identity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId1",
                schema: "Identity",
                table: "UserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId1",
                principalSchema: "Identity",
                principalTable: "Roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId1",
                schema: "Identity",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RoleId1",
                schema: "Identity",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                schema: "Identity",
                table: "UserRoles");
        }
    }
}
