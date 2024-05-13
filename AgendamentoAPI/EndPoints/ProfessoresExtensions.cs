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

            #region Endpoint Professores
            groupBuilder.MapGet("", ([FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] ProfessoresRequestEdit professoresRequestEdit) =>
            {
              
                var listaDeProfessores = dal.Listar();
                if (listaDeProfessores is null)
                {
                    return Results.NotFound();
                }
                var listaDeProfessoresResponse = listaDeProfessores.Select(a => new ProfessoresResponse(a.Id, a.Nome, a.email!)).ToList();
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


            groupBuilder.MapDelete("{id}", ([FromServices] DAL<Professores> dal, int id) => {
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

                var user = await userManager.FindByEmailAsync(professoresAAtualizar.email!);
                if (user == null)
                {
                    return Results.NotFound("Usuário não encontrado.");
                }

                // Verifica se a senha e a confirmação de senha correspondem
                if (professoresRequestEdit.senha != professoresRequestEdit.confirmacaoSenha)
                {
                    return Results.BadRequest(new { message = "A senha e a confirmação de senha não correspondem." });
                }

                // Atualiza o nome do professor
                professoresAAtualizar.Nome = professoresRequestEdit.nome;

                // Atualiza o email e a senha do usuário
                user.Email = professoresRequestEdit.email;
                user.UserName = professoresRequestEdit.nome;
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await userManager.ResetPasswordAsync(user, passwordResetToken, professoresRequestEdit.senha);

                if (!passwordChangeResult.Succeeded)
                {
                    return Results.BadRequest(passwordChangeResult.Errors.Select(x => x.Description));
                }

                dal.Atualizar(professoresAAtualizar);
                return Results.Ok("Professor atualizado com sucesso.");
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

            #endregion
        }

        private static ICollection<ProfessoresResponse> EntityListToResponseList(IEnumerable<Professores> listaDeProfessores)
        {

            return listaDeProfessores.Select(a => EntityToResponse(a)).ToList();
        }

        private static ProfessoresResponse EntityToResponse(Professores Professores)
        {
            return new ProfessoresResponse(Professores.Id, Professores.Nome, Professores.email!);
        }
    }
}
