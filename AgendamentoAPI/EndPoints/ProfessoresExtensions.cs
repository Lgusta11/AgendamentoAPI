using AgendamentosAPI.Shared.Dados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Agendamentos.EndPoints
{
    public static class ProfessoresExtensions
    {
        public static void AddEndPointsProfessores(this WebApplication app)
        {

          var groupBuilder = app.MapGroup("professores")
             .WithTags("Professores");

            groupBuilder.MapGet("",[Authorize] async ([FromServices] ProfessorService professorService) =>
            {
                var listaDeProfessores = await professorService.ListarProfessores();

                return Results.Ok(listaDeProfessores);
            });



             groupBuilder.MapGet("{id}", [Authorize]  async ([FromServices] ProfessorService professorService, string id) =>
             {
                 var professor = await professorService.BuscarProfessorPorId(p => p.Id == id);

                 if(professor is null) return Results.NotFound();

                 return Results.Ok(professor);
             });
         }
        }
    }
