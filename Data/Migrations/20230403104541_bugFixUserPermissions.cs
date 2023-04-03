using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class bugFixUserPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permissionLevel",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "permissions",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permissions",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "permissionLevel",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
