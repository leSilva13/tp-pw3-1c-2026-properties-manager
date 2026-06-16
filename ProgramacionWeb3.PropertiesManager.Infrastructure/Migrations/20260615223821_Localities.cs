using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Localities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalityId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Localities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Localities_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Localities",
                columns: new[] { "Id", "Name", "ProvinceId" },
                values: new object[,]
                {
                    { 1, "Ciudad de Buenos Aires", 1 },
                    { 2, "La Plata", 2 },
                    { 3, "San Fernando", 2 },
                    { 4, "Mar del Plata", 2 },
                    { 5, "Rosario", 21 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LocalityId",
                table: "Addresses",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Localities_ProvinceId",
                table: "Localities",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Localities_LocalityId",
                table: "Addresses",
                column: "LocalityId",
                principalTable: "Localities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Localities_LocalityId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "Localities");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_LocalityId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "LocalityId",
                table: "Addresses");
        }
    }
}
