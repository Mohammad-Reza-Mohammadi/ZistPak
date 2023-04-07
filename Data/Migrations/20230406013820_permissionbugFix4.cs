using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class permissionbugFix4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
