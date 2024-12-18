using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class user_add_fullname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fullname",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fullname",
                schema: "Identity",
                table: "Users");
        }
    }
}
