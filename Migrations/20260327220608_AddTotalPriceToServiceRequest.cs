using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoRvStar.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalPriceToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ServiceRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ServiceRequests");
        }
    }
}
