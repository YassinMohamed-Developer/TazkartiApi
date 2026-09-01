using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tazkarti.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoundToTicketPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Round",
                table: "TicketPasses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Round",
                table: "TicketPasses");
        }
    }
}
