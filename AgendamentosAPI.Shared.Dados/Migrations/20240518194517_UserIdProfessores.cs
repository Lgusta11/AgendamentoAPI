using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class UserIdProfessores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeAula",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "NomeEquipamento",
                table: "Agendamentos");

            migrationBuilder.RenameColumn(
                name: "NomeProfessor",
                table: "Agendamentos",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Agendamentos",
                newName: "NomeProfessor");

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
        }
    }
}
