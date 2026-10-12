using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionLibrosBlazor.Migrations
{
    /// <inheritdoc />
    public partial class PrestamoDevolucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDevolucion",
                table: "Prestamos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Cantidad",
                table: "Libros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "LibroId",
                keyValue: 1,
                column: "Cantidad",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Libros",
                keyColumn: "LibroId",
                keyValue: 2,
                column: "Cantidad",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaDevolucion",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "Cantidad",
                table: "Libros");
        }
    }
}
