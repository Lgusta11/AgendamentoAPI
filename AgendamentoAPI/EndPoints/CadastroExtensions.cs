using AgendamentoAPI.Requests;
using Agendamentos.Requests;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgendamentoAPI.EndPoints
{
    public static class CadastroExtensions
    {


        public static void AddEndPointsCadastro(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                 .RequireAuthorization()
                .WithTags("Autenticação");

            //ADMIN
            groupBuilder.MapPost("Cadastro/Admin", async ([FromServices] DAL<Admin> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] AdminRequest adminRequest) =>
            {
                // Verifique se o papel de administrador existe, se não, crie um
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new Admin { Name = "Admin", Nome = "Admin", Email = "admin@example.com", Senha = "AdminAFS24_" };
                    await roleManager.CreateAsync(adminRole);
                }

                var user = new PessoaComAcesso { UserName = adminRequest.Nome, Email = adminRequest.Email };
                var result = await userManager.CreateAsync(user, adminRequest.Senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                await userManager.AddToRoleAsync(user, "Admin");

                var admin = new Admin { Nome = adminRequest.Nome, Email = adminRequest.Email, Senha = adminRequest.Senha };
                dal.Adicionar(admin);

                return Results.Ok();
            });




            //PROFESSOR
            groupBuilder.MapPost("Cadastro/Professor", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromBody] ProfessoresRequest professoresRequest) =>
            {
                if (professoresRequest.senha != professoresRequest.confirmacaoSenha)
                {
                    return Results.BadRequest("A senha e a confirmação de senha não correspondem.");
                }

                var user = new PessoaComAcesso { UserName = professoresRequest.nome, Email = professoresRequest.email };
                var result = await userManager.CreateAsync(user, professoresRequest.senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                var Professores = new Professores(professoresRequest.nome);
                dal.Adicionar(Professores);
                return Results.Ok();
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });

        }
    }
}