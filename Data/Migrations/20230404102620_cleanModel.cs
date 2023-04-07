using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class cleanModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_OwnerId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Municipality_municipalityId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_ParetnEmployeeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ParetnEmployeeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ParetnEmployeeId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "municipalityId",
                table: "User",
                newName: "MunicipalityId");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "User",
                newName: "UserToken");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "User",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "User",
                newName: "UserPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "User",
                newName: "UserIsActive");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "User",
                newName: "UserImage");

            migrationBuilder.RenameColumn(
                name: "HashPassword",
                table: "User",
                newName: "UserPasswordHash");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "User",
                newName: "UserParetnEmployeeId");

            migrationBuilder.RenameColumn(
                name: "FullName_LastName",
                table: "User",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "FullName_FirstName",
                table: "User",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "User",
                newName: "UserGender");

            migrationBuilder.RenameIndex(
                name: "IX_User_municipalityId",
                table: "User",
                newName: "IX_User_MunicipalityId");

            migrationBuilder.RenameColumn(
                name: "Town",
                table: "Address",
                newName: "UserAddressTown");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Address",
                newName: "UserAddressStreet");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "Address",
                newName: "UserAddressPostalCode");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Address",
                newName: "UserAddressOwnerId");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Address",
                newName: "UserAddressTitle");

            migrationBuilder.RenameColumn(
                name: "AddressTitle",
                table: "Address",
                newName: "UserAddressCity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Address",
                newName: "UserAddressId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_OwnerId",
                table: "Address",
                newName: "IX_Address_UserAddressOwnerId");

            migrationBuilder.AddColumn<int>(
                name: "UserAge",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserFullName_UserFirstName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserFullName_UserLastName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserParetnEmployeeId",
                table: "User",
                column: "UserParetnEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_UserAddressOwnerId",
                table: "Address",
                column: "UserAddressOwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Municipality_MunicipalityId",
                table: "User",
                column: "MunicipalityId",
                principalTable: "Municipality",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UserParetnEmployeeId",
                table: "User",
                column: "UserParetnEmployeeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_User_UserAddressOwnerId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Municipality_MunicipalityId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UserParetnEmployeeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserParetnEmployeeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserAge",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserFullName_UserFirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserFullName_UserLastName",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "MunicipalityId",
                table: "User",
                newName: "municipalityId");

            migrationBuilder.RenameColumn(
                name: "UserToken",
                table: "User",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "User",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "UserPhoneNumber",
                table: "User",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "UserPasswordHash",
                table: "User",
                newName: "HashPassword");

            migrationBuilder.RenameColumn(
                name: "UserParetnEmployeeId",
                table: "User",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "FullName_FirstName");

            migrationBuilder.RenameColumn(
                name: "UserIsActive",
                table: "User",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "UserImage",
                table: "User",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "UserGender",
                table: "User",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "User",
                newName: "FullName_LastName");

            migrationBuilder.RenameIndex(
                name: "IX_User_MunicipalityId",
                table: "User",
                newName: "IX_User_municipalityId");

            migrationBuilder.RenameColumn(
                name: "UserAddressTown",
                table: "Address",
                newName: "Town");

            migrationBuilder.RenameColumn(
                name: "UserAddressTitle",
                table: "Address",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "UserAddressStreet",
                table: "Address",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "UserAddressPostalCode",
                table: "Address",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "UserAddressOwnerId",
                table: "Address",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserAddressCity",
                table: "Address",
                newName: "AddressTitle");

            migrationBuilder.RenameColumn(
                name: "UserAddressId",
                table: "Address",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Address_UserAddressOwnerId",
                table: "Address",
                newName: "IX_Address_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParetnEmployeeId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ParetnEmployeeId",
                table: "User",
                column: "ParetnEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_User_OwnerId",
                table: "Address",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Municipality_municipalityId",
                table: "User",
                column: "municipalityId",
                principalTable: "Municipality",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_ParetnEmployeeId",
                table: "User",
                column: "ParetnEmployeeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
