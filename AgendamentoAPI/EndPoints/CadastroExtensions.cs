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
using Microsoft.EntityFrameworkCore;

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
                    var adminRole = new Admin { Name = "Admin" };
                    await roleManager.CreateAsync(adminRole);
                }

                // Valide a senha
                if (adminRequest.Senha != adminRequest.ConfirmacaoSenha)
                {
                    return Results.BadRequest(new { message = "A senha e a confirmação de senha não correspondem." });
                }

                // Crie o usuário
                var user = new PessoaComAcesso { UserName = adminRequest.Nome, Email = adminRequest.Email };
                var result = await userManager.CreateAsync(user, adminRequest.Senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                // Atribua o usuário ao papel de administrador
                await userManager.AddToRoleAsync(user, "Admin");

                // Crie uma instância de Admin sem a propriedade Senha
                var admin = new Admin { Nome = adminRequest.Nome, Email = adminRequest.Email };
                dal.Adicionar(admin);

                return Results.Json(new { message = "Administrador registrado com sucesso." });
            });


            //PROFESSOR
            groupBuilder.MapPost("Cadastro/Professor", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] ProfessoresRequest professoresRequest) =>
            {
                if (professoresRequest.senha != professoresRequest.confirmacaoSenha)
                {
                    return Results.BadRequest(new { message = "A senha e a confirmação de senha não correspondem." });
                }

                var user = new PessoaComAcesso { UserName = professoresRequest.nome, Email = professoresRequest.email };
                var result = await userManager.CreateAsync(user, professoresRequest.senha);

                if (result.Succeeded)
                {
                    // Crie um novo professor e defina o UserId para o Id do usuário do Identity
                    var professor = new Professores(professoresRequest.nome) { UserId = user.Id.ToString() };
                    dal.Adicionar(professor);

                    // Atribua a função "Professores" ao usuário
                    await userManager.AddToRoleAsync(user, "Professores");

                    return Results.Json(new { message = "Professor registrado com sucesso." });
                }
                else
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });
        }
    }
}
