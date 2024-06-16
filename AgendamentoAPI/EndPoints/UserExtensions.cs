using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Dados.Database;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.EndPoints
{
    public static class UserExtensions
    {

        public static void AddEndPoinsUsers(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("users")
             .WithTags("Users");

            groupBuilder.MapGet("", [Authorize] async ([FromServices] UserService userService) =>
            {
                var usuarios = await userService.ListarUsers();

                return Results.Ok(usuarios);
            });

            groupBuilder.MapGet("{id}", [Authorize] async ([FromServices] UserService userService, string id) =>
            {
                var usuario = await userService.BuscarUserPorId(p => p.Id == id);

                return usuario;
            });

            groupBuilder.MapGet("niveisAcesso", [Authorize] async ([FromServices] NiveisdeAcessoService niveisdeAcessoService) =>
            {
                var niveis = await niveisdeAcessoService.ListarNiveisDeAcesso();

                return niveis;
            });

            groupBuilder.MapGet("niveisAcesso/{id}", [Authorize] async ([FromServices] NiveisdeAcessoService niveisdeAcessoService,string id) =>
            {
                var nivel = await niveisdeAcessoService.BuscarNivelDeAcesso(id);

                return nivel;
            });

            groupBuilder.MapDelete("{id}", [Authorize(Roles = "Gestor")] ([FromServices] DAL<User> dal, string id) =>
            {
                var user = dal.RecuperarPor(u => u.Id == id);
                if (user is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(user);
                return Results.NoContent();
            });

            groupBuilder.MapPut("{id}", [Authorize(Roles = "Gestor")] async ([FromServices] DAL<User> dal, string id, [FromBody] UserRequestEdit userRequestEdit) =>
            {
                var existingUser = dal.RecuperarPor(u => u.Id == id);
                if (existingUser == null)
                {
                    return Results.NotFound();
                }

                existingUser.UserName = userRequestEdit.UserName;
                existingUser.Email = userRequestEdit.Email;

                dal.Atualizar(existingUser);

                return Results.NoContent();
            });

        }

    }
}
