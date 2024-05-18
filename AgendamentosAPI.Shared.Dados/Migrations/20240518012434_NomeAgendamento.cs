using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class NomeAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomeAula",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeEquipamento",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NomeProfessor",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeAula",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "NomeEquipamento",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "NomeProfessor",
                table: "Agendamentos");
        }
    }
}
