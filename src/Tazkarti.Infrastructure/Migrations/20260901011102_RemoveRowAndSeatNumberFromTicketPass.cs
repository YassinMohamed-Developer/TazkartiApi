using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tazkarti.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRowAndSeatNumberFromTicketPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Row",
                table: "TicketPasses");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "TicketPasses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Row",
                table: "TicketPasses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "TicketPasses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
