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

namespace AgendamentoAPI.EndPoints
{
    public static class LoginExtensions
    {
        public static void AddEndPoinsLogin(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            groupBuilder.MapPost("Login", async ([FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] SignInManager<PessoaComAcesso> signInManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] LoginRequest loginRequest, IConfiguration _config) =>
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
                else if (roles.Contains("Professor"))
                {
                    redirectUrl = "/Home";
                }
                else
                {
                    redirectUrl = "/";
                }

                // Geração do Token JWT
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var jwtKey = _config["Jwt:Key"];
                if (jwtKey == null)
                {
                    throw new ArgumentNullException(nameof(jwtKey),
                                                        "JwtKey cannot be null.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), redirectUrl });
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


            groupBuilder.MapGet("manage/info", async (HttpContext context) =>
            {
                var userManager = context.RequestServices.GetRequiredService<UserManager<PessoaComAcesso>>();
                var user = await userManager.GetUserAsync(context.User);
                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var roles = await userManager.GetRolesAsync(user);

                var userInfo = new
                {
                    user.Email,
                    user.UserName,
                    Roles = roles
                };

                return Results.Ok(userInfo);
            });
        }
    }
}
