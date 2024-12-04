using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library_api.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdLivro",
                table: "Emprestimos");

            migrationBuilder.DropColumn(
                name: "IdLocatario",
                table: "Emprestimos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdLivro",
                table: "Emprestimos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdLocatario",
                table: "Emprestimos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
