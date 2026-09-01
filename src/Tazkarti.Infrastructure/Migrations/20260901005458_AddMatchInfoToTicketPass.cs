using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tazkarti.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchInfoToTicketPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayTeam",
                table: "TicketPasses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Competition",
                table: "TicketPasses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeTeam",
                table: "TicketPasses",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TicketPasses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeam",
                table: "TicketPasses");

            migrationBuilder.DropColumn(
                name: "Competition",
                table: "TicketPasses");

            migrationBuilder.DropColumn(
                name: "HomeTeam",
                table: "TicketPasses");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TicketPasses");
        }
    }
}
