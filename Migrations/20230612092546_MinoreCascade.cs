using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCar.Migrations
{
    /// <inheritdoc />
    public partial class MinoreCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_PhoneNumber",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteCars_Users_UserId",
                table: "UserFavoriteCars");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_PhoneNumber",
                table: "Purchases",
                column: "PhoneNumber",
                principalTable: "Users",
                principalColumn: "PhoneNumber",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteCars_Users_UserId",
                table: "UserFavoriteCars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "PhoneNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Users_PhoneNumber",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteCars_Users_UserId",
                table: "UserFavoriteCars");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Users_PhoneNumber",
                table: "Purchases",
                column: "PhoneNumber",
                principalTable: "Users",
                principalColumn: "PhoneNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteCars_Users_UserId",
                table: "UserFavoriteCars",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "PhoneNumber");
        }
    }
}
