using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SUM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultipurposeRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuildingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipurposeRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipurposeRooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipurposeRoomReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Shift = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WasWellDelivered = table.Column<bool>(type: "bit", nullable: false),
                    MultipurposeRoomId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipurposeRoomReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipurposeRoomReservations_MultipurposeRooms_MultipurposeRoomId",
                        column: x => x.MultipurposeRoomId,
                        principalTable: "MultipurposeRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MultipurposeRoomReservations_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipurposeRoomReservations_MultipurposeRoomId",
                table: "MultipurposeRoomReservations",
                column: "MultipurposeRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipurposeRoomReservations_UnitId",
                table: "MultipurposeRoomReservations",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipurposeRooms_BuildingId",
                table: "MultipurposeRooms",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultipurposeRoomReservations");

            migrationBuilder.DropTable(
                name: "MultipurposeRooms");
        }
    }
}
