using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentosAPI.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class Projetoinicia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "669c2647-e5e7-46db-b7fe-d15109c5d02f");

            migrationBuilder.DeleteData(
                table: "NivelAcessos",
                keyColumn: "Id",
                keyValue: "9e5ddf95-0954-4d64-8cf2-edbf36ff23ca");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "NivelAcessos",
                columns: new[] { "Id", "TipoAcesso" },
                values: new object[] { "db731824-8702-423b-bbed-97aa37ca9494", "Gestor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AcessoId", "Email", "Senha", "Token", "UserName" },
                values: new object[] { "7186557b-7a87-456c-bd10-7e1b829ffc1f", "db731824-8702-423b-bbed-97aa37ca9494", "root@gmail.com", "Soeuseisoeusei", null, "root" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "7186557b-7a87-456c-bd10-7e1b829ffc1f");

            migrationBuilder.DeleteData(
                table: "NivelAcessos",
                keyColumn: "Id",
                keyValue: "db731824-8702-423b-bbed-97aa37ca9494");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Senha",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "NivelAcessos",
                columns: new[] { "Id", "TipoAcesso" },
                values: new object[] { "9e5ddf95-0954-4d64-8cf2-edbf36ff23ca", "Gestor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AcessoId", "Email", "Senha", "Token", "UserName" },
                values: new object[] { "669c2647-e5e7-46db-b7fe-d15109c5d02f", "9e5ddf95-0954-4d64-8cf2-edbf36ff23ca", "root@gmail.com", "bnt8Ssp+p4gwubN2dXeYXpNURr/qr5QbPmpylnWgLtP0qZay", null, "root" });
        }
    }
}
