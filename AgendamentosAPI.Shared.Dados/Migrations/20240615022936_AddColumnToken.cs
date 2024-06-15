using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendamentosAPI.Shared.Dados.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "90c7a31e-572e-4007-9f36-aca3b660dbbc");

            migrationBuilder.DeleteData(
                table: "NivelAcessos",
                keyColumn: "Id",
                keyValue: "7334c9b6-fda8-4e99-9c61-bfcb272483c7");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "NivelAcessos",
                columns: new[] { "Id", "TipoAcesso" },
                values: new object[] { "918b0a58-4334-4af7-92a5-1e5d39064f88", "Gestor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AcessoId", "Email", "Senha", "Token", "UserName" },
                values: new object[] { "718dcaf6-1cb1-41e6-b747-8466f6a05134", "918b0a58-4334-4af7-92a5-1e5d39064f88", "root@gmail.com", "Soeuseisoeusei", null, "root" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "718dcaf6-1cb1-41e6-b747-8466f6a05134");

            migrationBuilder.DeleteData(
                table: "NivelAcessos",
                keyColumn: "Id",
                keyValue: "918b0a58-4334-4af7-92a5-1e5d39064f88");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.InsertData(
                table: "NivelAcessos",
                columns: new[] { "Id", "TipoAcesso" },
                values: new object[] { "7334c9b6-fda8-4e99-9c61-bfcb272483c7", "Gestor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AcessoId", "Email", "Senha", "UserName" },
                values: new object[] { "90c7a31e-572e-4007-9f36-aca3b660dbbc", "7334c9b6-fda8-4e99-9c61-bfcb272483c7", "root@gmail.com", "Soeuseisoeusei", "root" });
        }
    }
}
