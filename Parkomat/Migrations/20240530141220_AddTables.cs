using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parkomat.Migrations
{
    /// <inheritdoc />
    public partial class AddTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    PriceListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour1 = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Hour2 = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Hour3 = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Rest = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.PriceListId);
                });

            migrationBuilder.CreateTable(
                name: "ParkingsLots",
                columns: table => new
                {
                    ParkingLotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkingLotName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingsLots", x => x.ParkingLotId);
                    table.ForeignKey(
                        name: "FK_ParkingsLots_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "PriceListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parkings",
                columns: table => new
                {
                    ParkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarLicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParkingStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParkingStop = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ParkingLotID = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parkings", x => x.ParkingId);
                    table.ForeignKey(
                        name: "FK_Parkings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parkings_ParkingsLots_ParkingLotID",
                        column: x => x.ParkingLotID,
                        principalTable: "ParkingsLots",
                        principalColumn: "ParkingLotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_ParkingLotID",
                table: "Parkings",
                column: "ParkingLotID");

            migrationBuilder.CreateIndex(
                name: "IX_Parkings_UserId",
                table: "Parkings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingsLots_PriceListId",
                table: "ParkingsLots",
                column: "PriceListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parkings");

            migrationBuilder.DropTable(
                name: "ParkingsLots");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");
        }
    }
}
