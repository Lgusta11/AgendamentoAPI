using AgendamentoAPI.Email;
using Agendamentos.Requests;
using Agendamentos.Response;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Agendamentos.EndPoints
{
    public static class ProfessoresExtensions
    {
        public static void AddEndPointsProfessores(this WebApplication app)
        {

            var groupBuilder = app.MapGroup("professores")
            .WithTags("Professores");

            groupBuilder.MapGet("", async ([FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager) =>
            {
                var listaDeProfessores = dal.Listar();
                if (listaDeProfessores is null)
                {
                    return Results.NotFound();
                }
                var listaDeProfessoresResponse = new List<ProfessoresResponse>();
                foreach (var professor in listaDeProfessores)
                {
                    var user = await userManager.FindByIdAsync(professor.UserId);
                    if (user != null)
                    {
                        listaDeProfessoresResponse.Add(new ProfessoresResponse(professor.Id, professor.Nome, user.Email));
                    }
                    else
                    {
                        // Aqui você pode lidar com a situação em que o UserId não corresponde a nenhum usuário
                        // Por exemplo, você pode adicionar um log de erro ou lançar uma exceção
                        Console.WriteLine($"Erro: UserId {professor.UserId} não encontrado na tabela de usuários.");
                    }
                }
                return Results.Ok(listaDeProfessoresResponse);
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });



            groupBuilder.MapGet("{id}", ([FromServices] DAL<Professores> dal, int id) =>
            {
                var professor = dal.RecuperarPor(a => a.Id == id);
                if (professor is null)
                {
                    return Results.NotFound();
                }
                var professorResponse = new ProfessoresResponse(professor.Id, professor.Nome, professor.email!);
                return Results.Ok(professorResponse);
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });


            groupBuilder.MapDelete("{id}", ([FromServices] DAL<Professores> dal, int id) =>
            {
                var Professores = dal.RecuperarPor(a => a.Id == id);
                if (Professores is null)
                {
                    return Results.NotFound();
                }
                dal.Deletar(Professores);
                return Results.NoContent();

            });

            groupBuilder.MapPut("{id}", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] ProfessoresRequestEdit professoresRequestEdit) =>
            {
                var professoresAAtualizar = dal.RecuperarPor(a => a.Id == professoresRequestEdit.Id);
                if (professoresAAtualizar is null)
                {
                    return Results.NotFound();
                }

                var user = await userManager.FindByIdAsync(professoresAAtualizar.UserId);
                if (user == null)
                {
                    return Results.NotFound("Usuário não encontrado.");
                }

                // Atualiza o nome do professor
                professoresAAtualizar.Nome = professoresRequestEdit.nome;

                // Atualiza o email do usuário
                user.Email = professoresRequestEdit.email;
                user.UserName = professoresRequestEdit.nome;

                // Atualiza o email na tabela de usuários do Identity
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return Results.Problem("Não foi possível atualizar o email do usuário.");
                }

                dal.Atualizar(professoresAAtualizar);
                return Results.Ok("Professor atualizado com sucesso.");
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });





        }

        private static async Task<ICollection<ProfessoresResponse>> EntityListToResponseList(IEnumerable<Professores> listaDeProfessores, UserManager<PessoaComAcesso> userManager)
        {
            var tasks = listaDeProfessores.ToList().Select(a => EntityToResponse(a, userManager)).ToArray();
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }




        private static async Task<ProfessoresResponse> EntityToResponse(Professores Professores, UserManager<PessoaComAcesso> userManager)
        {
            var user = await userManager.FindByIdAsync(Professores.Id.ToString());
            return new ProfessoresResponse(Professores.Id, Professores.Nome, user?.Email);
        }
    }
    }
