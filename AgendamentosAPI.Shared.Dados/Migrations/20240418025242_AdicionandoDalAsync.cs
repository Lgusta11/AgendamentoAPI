using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoDalAsync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantidade",
                table: "Equipamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "Equipamentos");
        }
    }
}
