using Agendamentos.Requests;
using Agendamentos.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Agendamentos.EndPoints
{
    public static class ProfessoresExtensions
    {
        public static void AddEndPointsProfessores(this WebApplication app)
        {

            var groupBuilder = app.MapGroup("professores")
            .RequireAuthorization()
            .WithTags("Professores");

            #region Endpoint Professores
            groupBuilder.MapGet("", ([FromServices] DAL<Professores> dal) =>
            {
                var listaDeProfessores = dal.Listar();
                if (listaDeProfessores is null)
                {
                    return Results.NotFound();
                }
                var listaDeProfessoresResponse = EntityListToResponseList(listaDeProfessores);
                return Results.Ok(listaDeProfessoresResponse);
            });

            groupBuilder.MapGet("{nome}", ([FromServices] DAL<Professores> dal, string nome) =>
            {
                var professores = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
                if (professores is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(EntityToResponse(professores));

            });

            groupBuilder.MapPost("", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromBody] ProfessoresRequest professoresRequest) =>
            {

                var nome = professoresRequest.nome.Trim();
                var imagemProfessor = DateTime.Now.ToString("ddMMyyyyhhss") + "." + nome + ".jpg";

                var path = Path.Combine(env.ContentRootPath,
                    "wwwroot", "FotosPerfil", imagemProfessor);

                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(professoresRequest.fotoPerfil!));
                using FileStream fs = new(path, FileMode.Create);
                await ms.CopyToAsync(fs);

                var Professores = new Professores(professoresRequest.nome) { FotoPerfil = $"/FotosPerfil/{imagemProfessor}" };

                dal.Adicionar(Professores);
                return Results.Ok();
            });

            groupBuilder.MapDelete("{id}", ([FromServices] DAL<Professores> dal, int id) => {
                var Professores = dal.RecuperarPor(a => a.Id == id);
                if (Professores is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(Professores);
                return Results.NoContent();

            });

            groupBuilder.MapPut("", ([FromServices] DAL<Professores> dal, [FromBody] ProfessoresRequestEdit ProfessoresRequestEdit) => {
                var ProfessoresAAtualizar = dal.RecuperarPor(a => a.Id == ProfessoresRequestEdit.Id);
                if (ProfessoresAAtualizar is null)
                {
                    return Results.NotFound();
                }
                ProfessoresAAtualizar.Nome = ProfessoresRequestEdit.nome;
                dal.Atualizar(ProfessoresAAtualizar);
                return Results.Ok();
            });
            #endregion
        }

        private static ICollection<ProfessoresResponse> EntityListToResponseList(IEnumerable<Professores> listaDeProfessores)
        {
            return listaDeProfessores.Select(a => EntityToResponse(a)).ToList();
        }

        private static ProfessoresResponse EntityToResponse(Professores Professores)
        {
            return new ProfessoresResponse(Professores.Id, Professores.Nome, Professores.FotoPerfil);
        }
    }
}
