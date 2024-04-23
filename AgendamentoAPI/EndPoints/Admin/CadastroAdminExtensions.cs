using AgendamentoAPI.Requests;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.EndPoints.AdminCrud
{
    public static class CadastroAdminExtensions
    {
        public static void AddEndPointsAdmin(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("Admin")
               .RequireAuthorization()
               .WithTags("Admin");

                groupBuilder.MapGet("", ([FromServices] DAL<Admin> dal) =>
                {
                    var listaDeAdmins = dal.Listar();
                    if (listaDeAdmins is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(listaDeAdmins);
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

            groupBuilder.MapGet("{id}", ([FromServices] DAL<Admin> dal, int id) =>
                {
                    var admin = dal.RecuperarPor(a => a.Id == id);
                    if (admin is null)
                    {
                        return Results.NotFound();
                    }
                    return Results.Ok(admin);
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

           

            groupBuilder.MapPut("{id}", async ([FromServices] DAL<Admin> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromBody] AdminRequestEdit adminRequestEdit, int id) =>
                {
                    var adminAAtualizar = dal.RecuperarPor(a => a.Id == id);
                    if (adminAAtualizar is null)
                    {
                        return Results.NotFound();
                    }

                    var user = await userManager.FindByEmailAsync(adminRequestEdit.Email);
                    if (user is null)
                    {
                        return Results.NotFound("Usuário não encontrado.");
                    }

                    user.Email = adminRequestEdit.Email;
                    user.UserName = adminRequestEdit.Email;

                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        return Results.BadRequest(result.Errors.Select(x => x.Description));
                    }

                    adminAAtualizar.Nome = adminRequestEdit.Nome;
                    dal.Atualizar(adminAAtualizar);

                    return Results.Ok();
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

            groupBuilder.MapDelete("{id}", ([FromServices] DAL<Admin> dal, int id) =>
                {
                    var admin = dal.RecuperarPor(a => a.Id == id);
                    if (admin is null)
                    {
                        return Results.NotFound();
                    }
                    dal.Deletar(admin);
                    return Results.NoContent();
                }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });
        }
        }
    }



