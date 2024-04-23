using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoFotoIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Professores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Professores");
        }
    }
}
