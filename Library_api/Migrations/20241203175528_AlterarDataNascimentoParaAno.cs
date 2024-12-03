using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_api.Migrations
{
    /// <inheritdoc />
    public partial class AlterarDataNascimentoParaAno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Locatarios");

            migrationBuilder.AddColumn<int>(
                name: "AnoNascimento",
                table: "Locatarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnoNascimento",
                table: "Locatarios");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Locatarios",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
