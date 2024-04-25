using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Agendamentos.Shared.Modelos.Modelos;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Requests;
using AgendamentoAPI.Requests;
using Microsoft.AspNetCore.Authorization;

namespace AgendamentoAPI.EndPoints
{
    public static class CadastroExtensions
    {
        public static void AddEndPointsCadastro(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            //ADMIN
            groupBuilder.MapPost("Cadastro/admin", async ([FromServices] DAL<Admin> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] AdminRequest adminRequest) =>
            {
                // Verifique se o papel de administrador existe, se não, crie um
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole<int> { Name = "Admin" };
                    await roleManager.CreateAsync((Admin)adminRole);
                }

                if (adminRequest.Senha != adminRequest.ConfirmacaoSenha)
                {
                    return Results.BadRequest(new { message = "A senha e a confirmação de senha não correspondem." });
                }

                var user = new PessoaComAcesso { UserName = adminRequest.Email, Email = adminRequest.Email };
                var result = await userManager.CreateAsync(user, adminRequest.Senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                await userManager.AddToRoleAsync(user, "Admin");

                // Crie uma instância de Admin sem a propriedade Senha
                var admin = new Admin { Nome = adminRequest.Nome, Email = adminRequest.Email };
                dal.Adicionar(admin);

                return Results.Ok("Administrador registrado com sucesso.");
            });

            //PROFESSOR
            groupBuilder.MapPost("Cadastro/Professor", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<IdentityRole> roleManager, [FromBody] ProfessoresRequest professoresRequest) =>
            {
                if (professoresRequest.senha != professoresRequest.confirmacaoSenha)
                {
                    return Results.BadRequest(new { message = "A senha e a confirmação de senha não correspondem." });
                }

                var user = new PessoaComAcesso { UserName = professoresRequest.nome, Email = professoresRequest.email };
                var result = await userManager.CreateAsync(user, professoresRequest.senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                // Verifique se a função "Professores" existe, se não, crie-a
                if (!await roleManager.RoleExistsAsync("Professores"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Professores"));
                }

                // Atribua a função "Professores" ao usuário
                await userManager.AddToRoleAsync(user, "Professores");

                var Professores = new Professores(professoresRequest.nome);
                dal.Adicionar(Professores);

                return Results.Ok("Professor registrado com sucesso.");
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });
        }
    }
}
