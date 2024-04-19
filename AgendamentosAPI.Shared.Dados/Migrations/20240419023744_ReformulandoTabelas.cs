using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class ReformulandoTabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Roles");
        }
    }
}
