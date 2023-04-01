using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class UserBugFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParentEmployeeId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ParentEmployeeId",
                table: "User",
                newName: "ParetnEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_User_ParentEmployeeId",
                table: "User",
                newName: "IX_User_ParetnEmployeeId");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParetnEmployeeId",
                table: "User",
                column: "ParetnEmployeeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParetnEmployeeId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "ParetnEmployeeId",
                table: "User",
                newName: "ParentEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_User_ParetnEmployeeId",
                table: "User",
                newName: "IX_User_ParentEmployeeId");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParentEmployeeId",
                table: "User",
                column: "ParentEmployeeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
