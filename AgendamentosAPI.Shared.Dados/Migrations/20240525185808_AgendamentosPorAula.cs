using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agendamentos.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AgendamentosPorAula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Aulas_AulaId",
                table: "Agendamentos");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_AulaId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "AulaId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Agendamentos");

            migrationBuilder.AddColumn<int>(
                name: "AulasId",
                table: "Agendamentos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AgendamentoAulas",
                columns: table => new
                {
                    AgendamentoId = table.Column<int>(type: "int", nullable: false),
                    AulaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgendamentoAulas", x => new { x.AgendamentoId, x.AulaId });
                    table.ForeignKey(
                        name: "FK_AgendamentoAulas_Agendamentos_AgendamentoId",
                        column: x => x.AgendamentoId,
                        principalTable: "Agendamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgendamentoAulas_Aulas_AulaId",
                        column: x => x.AulaId,
                        principalTable: "Aulas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_AulasId",
                table: "Agendamentos",
                column: "AulasId");

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoAulas_AulaId",
                table: "AgendamentoAulas",
                column: "AulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Aulas_AulasId",
                table: "Agendamentos",
                column: "AulasId",
                principalTable: "Aulas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Aulas_AulasId",
                table: "Agendamentos");

            migrationBuilder.DropTable(
                name: "AgendamentoAulas");

            migrationBuilder.DropIndex(
                name: "IX_Agendamentos_AulasId",
                table: "Agendamentos");

            migrationBuilder.DropColumn(
                name: "AulasId",
                table: "Agendamentos");

            migrationBuilder.AddColumn<int>(
                name: "AulaId",
                table: "Agendamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Agendamentos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_AulaId",
                table: "Agendamentos",
                column: "AulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Aulas_AulaId",
                table: "Agendamentos",
                column: "AulaId",
                principalTable: "Aulas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
