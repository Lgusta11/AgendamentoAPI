using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgendamentosAPI.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class Projetoinicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Aula = table.Column<string>(type: "text", nullable: true),
                    Duracao = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NivelAcessos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TipoAcesso = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelAcessos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false),
                    AcessoId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Acesso",
                        column: x => x.AcessoId,
                        principalTable: "NivelAcessos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EquipamentoId = table.Column<int>(type: "integer", nullable: false),
                    ProfessorId = table.Column<string>(type: "text", nullable: false),
                    AulasId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Aulas_AulasId",
                        column: x => x.AulasId,
                        principalTable: "Aulas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agendamentos_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Users_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgendamentoAulas",
                columns: table => new
                {
                    AgendamentoId = table.Column<int>(type: "integer", nullable: false),
                    AulaId = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.InsertData(
                table: "NivelAcessos",
                columns: new[] { "Id", "TipoAcesso" },
                values: new object[] { "9e5ddf95-0954-4d64-8cf2-edbf36ff23ca", "Gestor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AcessoId", "Email", "Senha", "Token", "UserName" },
                values: new object[] { "669c2647-e5e7-46db-b7fe-d15109c5d02f", "9e5ddf95-0954-4d64-8cf2-edbf36ff23ca", "root@gmail.com", "bnt8Ssp+p4gwubN2dXeYXpNURr/qr5QbPmpylnWgLtP0qZay", null, "root" });

            migrationBuilder.CreateIndex(
                name: "IX_AgendamentoAulas_AulaId",
                table: "AgendamentoAulas",
                column: "AulaId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_AulasId",
                table: "Agendamentos",
                column: "AulasId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_EquipamentoId",
                table: "Agendamentos",
                column: "EquipamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_ProfessorId",
                table: "Agendamentos",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AcessoId",
                table: "Users",
                column: "AcessoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgendamentoAulas");

            migrationBuilder.DropTable(
                name: "Agendamentos");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "NivelAcessos");
        }
    }
}
