using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class addMunicipalityPermissinosTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UPermissions_Municipality_municiaplityId",
                table: "UPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UPermissions_municiaplityId",
                table: "UPermissions");

            migrationBuilder.DropColumn(
                name: "municiaplityId",
                table: "UPermissions");

            migrationBuilder.CreateTable(
                name: "MunicipalityPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    municiaplityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalityPermissions_Municipality_municiaplityId",
                        column: x => x.municiaplityId,
                        principalTable: "Municipality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityPermissions_municiaplityId",
                table: "MunicipalityPermissions",
                column: "municiaplityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MunicipalityPermissions");

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
        }
    }
}
