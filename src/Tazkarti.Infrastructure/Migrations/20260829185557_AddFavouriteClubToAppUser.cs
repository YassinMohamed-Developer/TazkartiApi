using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tazkarti.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouriteClubToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavouriteClubId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FavouriteClubId",
                table: "AspNetUsers",
                column: "FavouriteClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clubs_FavouriteClubId",
                table: "AspNetUsers",
                column: "FavouriteClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clubs_FavouriteClubId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FavouriteClubId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FavouriteClubId",
                table: "AspNetUsers");
        }
    }
}
