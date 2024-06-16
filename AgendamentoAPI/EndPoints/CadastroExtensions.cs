using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Agendamentos.Shared.Modelos.Modelos;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Requests;
using AgendamentoAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using AgendamentosAPI.Shared.Models.Modelos;

namespace AgendamentoAPI.EndPoints
{
    public static class CadastroExtensions
    {
        public static void AddEndPointsCadastro(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");


            groupBuilder.MapPost("Cadastro/Usuarios", [Authorize(Roles = "Gestor")] async ([FromServices] DAL<User> dal, [FromBody] UserRequest userRequest) =>
            {
                var user = new User(null, userRequest.UserName, userRequest.Email, userRequest.Password, userRequest.AcessoId);

                dal.Adicionar(user);

                return Results.Ok(user);

            });

            groupBuilder.MapPost("Cadastro/NivelAcesso", [Authorize(Roles = "Gestor")] ([FromServices] DAL<NivelAcesso> dal, [FromBody] NivelAcessoRequest nivelAcessoRequest) =>
            {
                var nivelAcesso = new NivelAcesso(null, nivelAcessoRequest.Acesso);

                dal.Adicionar(nivelAcesso);

                return Results.Ok(new
                {
                    Status = "Adicionado com sucesso!"
                });
            });
        }
    }
}
