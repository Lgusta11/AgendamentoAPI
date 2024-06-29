using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentosAPI.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AulaAgendamentoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Data",
                table: "Agendamentos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "AulaId",
                table: "Agendamentos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AulaId",
                table: "Agendamentos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Data",
                table: "Agendamentos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
