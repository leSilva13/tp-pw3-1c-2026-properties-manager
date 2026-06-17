using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProgramacionWeb3.PropertiesManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$r86SfDwts13XN5cd/WbjieGK0xYO1NxhHBt1dQUxNqD.pUqOGVSfu");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreationDate", "Email", "IsActive", "LastLoginDate", "Password", "RoleId" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "propertiesmanager@propertiesmanager.com", true, null, "$2a$11$r86SfDwts13XN5cd/WbjieGK0xYO1NxhHBt1dQUxNqD.pUqOGVSfu", 2 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "owner@propertiesmanager.com", true, null, "$2a$11$r86SfDwts13XN5cd/WbjieGK0xYO1NxhHBt1dQUxNqD.pUqOGVSfu", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$gavlfviBbYC2cqZXK3xZ4uJarljrpHMwBveXLtYz5FHlp3cJhz4S2");
        }
    }
}
