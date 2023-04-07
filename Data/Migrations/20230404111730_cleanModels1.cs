using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class cleanModels1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Cargo_cargoId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Municipality_MunicipalityId",
                table: "User");

            migrationBuilder.DropTable(
                name: "MunicipalityPermissions");

            migrationBuilder.DropTable(
                name: "Municipality");

            migrationBuilder.DropIndex(
                name: "IX_User_MunicipalityId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MunicipalityId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "cargoId",
                table: "OrderDetail",
                newName: "CargoId");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "OrderDetail",
                newName: "StarCargo");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_cargoId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_CargoId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Order",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RatingOreder",
                table: "Order",
                newName: "OrderStar");

            migrationBuilder.RenameColumn(
                name: "Whight",
                table: "Item",
                newName: "ItemWhight");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Item",
                newName: "ItemStar");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Item",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Cargo",
                newName: "CargoStatus");

            migrationBuilder.RenameColumn(
                name: "Whight",
                table: "Cargo",
                newName: "CargoWhight");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Cargo",
                newName: "CargoStar");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cargo",
                newName: "CargoName");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Cargo",
                newName: "CargoCount");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Cargo_CargoId",
                table: "OrderDetail",
                column: "CargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Cargo_CargoId",
                table: "OrderDetail");

            migrationBuilder.RenameColumn(
                name: "CargoId",
                table: "OrderDetail",
                newName: "cargoId");

            migrationBuilder.RenameColumn(
                name: "StarCargo",
                table: "OrderDetail",
                newName: "rating");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_CargoId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_cargoId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Order",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "OrderStar",
                table: "Order",
                newName: "RatingOreder");

            migrationBuilder.RenameColumn(
                name: "ItemWhight",
                table: "Item",
                newName: "Whight");

            migrationBuilder.RenameColumn(
                name: "ItemStar",
                table: "Item",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Item",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CargoWhight",
                table: "Cargo",
                newName: "Whight");

            migrationBuilder.RenameColumn(
                name: "CargoStatus",
                table: "Cargo",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "CargoStar",
                table: "Cargo",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "CargoName",
                table: "Cargo",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CargoCount",
                table: "Cargo",
                newName: "Count");

            migrationBuilder.AddColumn<int>(
                name: "MunicipalityId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashpassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<int>(type: "int", nullable: false),
                    UpdateDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MunicipalityPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    municipalityId = table.Column<int>(type: "int", nullable: false),
                    Permission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    municiaplityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MunicipalityPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MunicipalityPermissions_Municipality_municipalityId",
                        column: x => x.municipalityId,
                        principalTable: "Municipality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_MunicipalityId",
                table: "User",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MunicipalityPermissions_municipalityId",
                table: "MunicipalityPermissions",
                column: "municipalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Cargo_cargoId",
                table: "OrderDetail",
                column: "cargoId",
                principalTable: "Cargo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Municipality_MunicipalityId",
                table: "User",
                column: "MunicipalityId",
                principalTable: "Municipality",
                principalColumn: "Id");
        }
    }
}
