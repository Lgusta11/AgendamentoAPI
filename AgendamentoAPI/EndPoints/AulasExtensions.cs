using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.EndPoints
{
    public static class AulasExtensions 
    {
        public static void AddEndPointsAulas(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("aulas")
            .WithTags("Aulas");

            #region Endpoint Aulas
            groupBuilder.MapGet("",[Authorize] ([FromServices] DAL<Aulas> dal) =>
            {
                var listaDeAulas = dal.Listar();
                if (listaDeAulas is null)
                {
                    return Results.NotFound();
                }
                var listaDeAulasResponse = EntityListToResponseList(listaDeAulas);
                return Results.Ok(listaDeAulasResponse);
            });

            groupBuilder.MapGet("{id}",[Authorize] ([FromServices] DAL<Aulas> dal, int id) =>
            {
                var aula = dal.RecuperarPor(a => a.Id == id);
                if (aula is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(EntityToResponse(aula));
            });

            groupBuilder.MapPost("", [Authorize(Roles = "Gestor")] ([FromServices] DAL<Aulas> dal, [FromBody] AulasRequest aulasRequest) =>
            {
                var aulaExistente = dal.RecuperarPor(a => a.Aula == aulasRequest.Aula);
                if (aulaExistente != null)
                {
                    return Results.BadRequest("A aula já existe.");
                }

                var totalAulas = dal.Listar().Count();
                if (totalAulas >= 10)
                {
                    return Results.BadRequest("Não é possível criar mais de 10 aulas.");
                }

                var aula = new Aulas(aulasRequest.Aula) { Duracao = TimeSpan.FromMinutes(50) };
                dal.Adicionar(aula);
                return Results.Ok();
            });

            groupBuilder.MapDelete("{id}", [Authorize(Roles = "Gestor")] ([FromServices] DAL<Aulas> dal, int id) =>
            {
                var aula = dal.RecuperarPor(a => a.Id == id);
                if (aula is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(aula);
                return Results.NoContent();
            });

            groupBuilder.MapPut("", [Authorize(Roles = "Gestor")] ([FromServices] DAL<Aulas> dal, [FromBody] AulasRequestEdit aulasRequestEdit) =>
            {
                var aulaAAtualizar = dal.RecuperarPor(a => a.Id == aulasRequestEdit.Id);
                if (aulaAAtualizar is null)
                {
                    return Results.NotFound();
                }
                aulaAAtualizar.Aula = aulasRequestEdit.Aula;
                dal.Atualizar(aulaAAtualizar);
                return Results.Ok();
            });
            #endregion
        }

        private static ICollection<AulasResponse> EntityListToResponseList(IEnumerable<Aulas> listaDeAulas)
        {
            return listaDeAulas.Select(a => EntityToResponse(a)).ToList();
        }

        private static AulasResponse EntityToResponse(Aulas aula)
        {
            return new AulasResponse(aula.Id, aula.Aula!, aula.Duracao);
        }
    }
}

