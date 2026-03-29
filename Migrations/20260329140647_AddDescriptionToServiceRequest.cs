using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoRvStar.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ServiceRequests",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ServiceRequests");
        }
    }
}
