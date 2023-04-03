using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class permissionsMunicaipalityadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions");

            migrationBuilder.AddColumn<int>(
                name: "municiaplityId",
                table: "UPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UPermissions_municiaplityId",
                table: "UPermissions",
                column: "municiaplityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UPermissions_Municipality_municiaplityId",
                table: "UPermissions",
                column: "municiaplityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_Municipality_municiaplityId",
                table: "UPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UPermissions_municiaplityId",
                table: "UPermissions");

            migrationBuilder.DropColumn(
                name: "municiaplityId",
                table: "UPermissions");

            migrationBuilder.AddForeignKey(
                name: "FK_UPermissions_User_userId",
                table: "UPermissions",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
