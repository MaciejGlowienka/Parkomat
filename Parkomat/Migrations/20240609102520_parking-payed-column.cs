using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parkomat.Migrations
{
    /// <inheritdoc />
    public partial class parkingpayedcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Parkings",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Parkings");
        }
    }
}
