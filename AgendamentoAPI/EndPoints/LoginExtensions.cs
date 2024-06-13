using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AgendamentoAPI.EndPoints
{
    public static class LoginExtensions
    {
        public static void AddEndPoinsLogin(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            groupBuilder.MapPost("Login", async ([FromServices] UserManager<PessoaComAcesso> userManager,
                                      [FromServices] SignInManager<PessoaComAcesso> signInManager,
                                      [FromBody] LoginRequest loginRequest,
                                      [FromServices] IConfiguration _config) =>
            {
                var user = await userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    return Results.BadRequest(new { message = "Usuário não encontrado." });
                }

                var result = await signInManager.PasswordSignInAsync(user, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return Results.BadRequest(new { message = "Falha na autenticação." });
                }

                // Verifique a função do usuário
                var roles = await userManager.GetRolesAsync(user);
                string redirectUrl;
                if (roles.Contains("Admin"))
                {
                    redirectUrl = "/Admin/Home";
                }
                else if (roles.Contains("Professores"))
                {
                    redirectUrl = "/Home";
                }
                else
                {
                    redirectUrl = "/";
                }

                // Crie as claims do usuário
                var claims = new List<Claim>
                 {
                 new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                // Adicione as roles às claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // Crie a identidade do usuário e defina os cookies de autenticação
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Defina o tempo de expiração conforme necessário
                };

                await signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return Results.Ok(new
                {
                    message = "Autenticação bem-sucedida.",
                    redirectUrl
                });
            });



            groupBuilder.MapGet("GetRoles/{emailOrUserName}", async ([FromServices] UserManager<PessoaComAcesso> userManager, string emailOrUserName) =>
            {
                var user = await userManager.FindByEmailAsync(emailOrUserName);
                if (user == null)
                {
                    user = await userManager.FindByNameAsync(emailOrUserName);
                }

                if (user == null)
                {
                    return Results.NotFound(new { message = "Usuário não encontrado." });
                }

                var roles = await userManager.GetRolesAsync(user);
                return Results.Ok(new { roles });
            });

            groupBuilder.MapGet("GetUserName/{email}", async ([FromServices] UserManager<PessoaComAcesso> userManager, string email) =>
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Results.NotFound(new { message = "Usuário não encontrado." });
                }

                return Results.Ok(new { userName = user.UserName });
            });

            groupBuilder.MapGet("user/information", async (HttpContext context) =>
            {
                var userManager = context.RequestServices.GetRequiredService<UserManager<PessoaComAcesso>>();
                var user = await userManager.GetUserAsync(context.User);
                if (user == null)
                {
                    return Results.NotFound(new { message = "Usuário não encontrado." });
                }

                var roles = await userManager.GetRolesAsync(user);

                var userInfo = new
                {
                    user.Id,
                    user.Email,
                    user.UserName,
                    Roles = roles
                };

                return Results.Ok(userInfo);
            }).RequireAuthorization();

            groupBuilder.MapGet("autenticado", async (HttpContext context) =>
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    await context.Response.WriteAsJsonAsync(new { Status = "Autenticado", User = context.User.Identity.Name });
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { Status = "Não autenticado" });
                }
            });
        }
    }
}