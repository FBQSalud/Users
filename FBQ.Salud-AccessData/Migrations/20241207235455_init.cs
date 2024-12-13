using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBQ.Salud_AccessData.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admin_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DNI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "RolId", "Name" },
                values: new object[] { 1, "Administrador" });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "RolId", "Name" },
                values: new object[] { 2, "Medico" });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "RolId", "Name" },
                values: new object[] { 3, "Recepcionista" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DNI", "Email", "EmployeeId", "FechaAlta", "Password", "Picture", "RolId", "SoftDelete", "UserName" },
                values: new object[,]
                {
                    { 1, "", "fbq.salud@gmail.com", 1, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7382), "admin", "claudio.jpg", 1, false, "admin" },
                    { 2, "41389372", "marianocarrizo@gmail.com", 2, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7392), "carrizo", "foto.jpg", 1, false, "mariano" },
                    { 3, "41389373", "medic@gmail.com", 3, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7393), "medic", "avatar-01.jpg", 2, false, "medico" },
                    { 4, "41389376", "segundez@gmail.com", 4, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7395), "segundo", "avatar-05.jpg", 2, false, "segundez" },
                    { 5, "41389379", "shrek@gmail.com", 5, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7396), "tercero", "avatar-03.jpg", 2, false, "tercerez" },
                    { 6, "41389379", "krokotilde@gmail.com", 6, new DateTime(2024, 12, 7, 20, 54, 55, 140, DateTimeKind.Local).AddTicks(7397), "kotil", "avatar-02.jpg", 3, false, "clotilde" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admin_RolId",
                table: "Admin",
                column: "RolId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId",
                table: "Users",
                column: "RolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
