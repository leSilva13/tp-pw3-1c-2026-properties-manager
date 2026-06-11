using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AgregarUserSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "IsActive", "LastLoginDate", "Password", "RoleId" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@propertiesmanager.com", true, null, "123", 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
