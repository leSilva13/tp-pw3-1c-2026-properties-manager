using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsuarioNuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "RoleId" },
                values: new object[] { "$2a$11$gavlfviBbYC2cqZXK3xZ4uJarljrpHMwBveXLtYz5FHlp3cJhz4S2", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Password", "RoleId" },
                values: new object[] { "123", 2 });
        }
    }
}
