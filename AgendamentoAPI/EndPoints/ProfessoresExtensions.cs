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
            groupBuilder.MapGet("", ([FromServices] DAL<Professores> dal) =>
            {
                var listaDeProfessores = dal.Listar();
                if (listaDeProfessores is null)
                {
                    return Results.NotFound();
                }
                var listaDeProfessoresResponse = EntityListToResponseList(listaDeProfessores);
                return Results.Ok(listaDeProfessoresResponse);
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

            groupBuilder.MapGet("{nome}", ([FromServices] DAL<Professores> dal, string nome) =>
            {
                var professores = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));
                if (professores is null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(EntityToResponse(professores));

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

            groupBuilder.MapPut("", async ([FromServices] DAL<Professores> dal, [FromServices] UserManager<IdentityUser> userManager, [FromBody] ProfessoresRequestEdit professoresRequestEdit) =>
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

                // Atualiza o nome do professor
                professoresAAtualizar.Nome = professoresRequestEdit.nome;

                // Atualiza o email e a senha do usuário
                user.Email = professoresRequestEdit.email;
                user.UserName = professoresRequestEdit.email;
                var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await userManager.ResetPasswordAsync(user, passwordResetToken, professoresRequestEdit.senha);

                if (!passwordChangeResult.Succeeded)
                {
                    return Results.BadRequest(passwordChangeResult.Errors.Select(x => x.Description));
                }

                dal.Atualizar(professoresAAtualizar);
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
