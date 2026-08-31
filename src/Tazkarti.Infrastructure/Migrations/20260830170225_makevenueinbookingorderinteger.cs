using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tazkarti.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class makevenueinbookingorderinteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Venue",
                table: "BookingOrders");

            migrationBuilder.AddColumn<int>(
                name: "VenueId",
                table: "BookingOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VenueId",
                table: "BookingOrders");

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                table: "BookingOrders",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
