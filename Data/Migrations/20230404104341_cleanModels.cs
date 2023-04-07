using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class cleanModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityPermissions_Municipality_municiaplityId",
                table: "MunicipalityPermissions");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityPermissions_municiaplityId",
                table: "MunicipalityPermissions");

            migrationBuilder.AddColumn<int>(
                name: "municipalityId",
                table: "MunicipalityPermissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "Municipality",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityPermissions_municipalityId",
                table: "MunicipalityPermissions",
                column: "municipalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityPermissions_Municipality_municipalityId",
                table: "MunicipalityPermissions",
                column: "municipalityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MunicipalityPermissions_Municipality_municipalityId",
                table: "MunicipalityPermissions");

            migrationBuilder.DropIndex(
                name: "IX_MunicipalityPermissions_municipalityId",
                table: "MunicipalityPermissions");

            migrationBuilder.DropColumn(
                name: "municipalityId",
                table: "MunicipalityPermissions");

            migrationBuilder.AlterColumn<string>(
                name: "token",
                table: "Municipality",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityPermissions_municiaplityId",
                table: "MunicipalityPermissions",
                column: "municiaplityId");

            migrationBuilder.AddForeignKey(
                name: "FK_MunicipalityPermissions_Municipality_municiaplityId",
                table: "MunicipalityPermissions",
                column: "municiaplityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
