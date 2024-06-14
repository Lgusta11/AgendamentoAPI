using AgendamentoAPI.Auth;
using AgendamentoAPI.Requests;
using AgendamentosAPI.Shared.Dados.Database.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoAPI.EndPoints
{
    public static class LoginExtensions
    {
        public static void AddEndPoinsLogin(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            groupBuilder.MapPost("Login", async ([FromServices] AuthService authService, [FromServices] ITokenService tokenService,[FromBody] LoginRequest login) =>
            {
                var usuarioExists = await authService.ValidarLogin(login.Email,login.Senha);

                if (usuarioExists) {

                    var token = tokenService.GetToken(login.Email,login.Senha);

                    return Results.Ok(new
                    {
                        login.Email,
                        Token = token
                    });
                }

                return Results.BadRequest(new
                {
                    login.Email,
                    Status = "Não encontrado!"
                });
            });
        }
    }
}