using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoNomeIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Users");
        }
    }
}
