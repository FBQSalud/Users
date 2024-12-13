using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBQ.Salud_AccessData.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3066));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3077));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3078));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3079));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3080));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 13, 18, 27, 44, 356, DateTimeKind.Local).AddTicks(3081));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7382));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7392));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7393));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7395));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7396));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6,
                column: "FechaAlta",
                value: new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7397));
        }
    }
}
