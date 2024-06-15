using AgendamentoAPI.Auth;
using AgendamentoAPI.Requests;
using AgendamentoAPI.Response;
using AgendamentosAPI.Shared.Dados.Database;
using AgendamentosAPI.Shared.Dados.Database.Auth;
using Microsoft.AspNetCore.Authorization;
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

            groupBuilder.MapPost("Login", async ([FromServices] AuthService authService,[FromServices] UserService userService, [FromServices] ITokenService tokenService,[FromBody] LoginRequest login) =>
            {
                var usuarioExists = await authService.ValidarLogin(login.Email,login.Senha);

                if (usuarioExists) {

                    var token = await tokenService.GetToken(login.Email,login.Senha);

                    var usuario = await userService.BuscarUserPorId(p => p.Email ==  login.Email && p.Senha == login.Senha);

                    usuario.AlterarToken(token);

                    await userService.AlterarUsuario(usuario);

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

            groupBuilder.MapGet("AutenticadoInfo",[Authorize] async ([FromServices] UserService userService, [FromBody] RequestToken requestToken) =>
            {
                var usuario = await userService.BuscarUserPorId(p => p.Token == requestToken.Token);

                return Results.Ok(new UserResponse(usuario.Id,usuario.Email, usuario.Senha));
            });
        }
    }
}