using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AgendamentoAPI.EndPoints
{
    public static class LoginExtensions
    {
        public static void AddEndPoinsLogin(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            groupBuilder.MapPost("Login", async ([FromServices] UserManager<PessoaComAcesso> userManager, [FromServices] RoleManager<Admin> roleManager, [FromBody] LoginRequest loginRequest, IConfiguration _config) =>
            {
                var user = await userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    return Results.Redirect("/login");
                }

                var result = await userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (!result)
                {
                    return Results.Redirect("/home");
                }

                // Verifique a função do usuário
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    return Results.Redirect("/Admin/Home");  // Alterado para "/Admin/Home"
                }
                else if (roles.Contains("Professor"))
                {
                    return Results.Redirect("/Home");  // Alterado para "/Home"
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
                    throw new ArgumentNullException(nameof(jwtKey), "JwtKey cannot be null.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            });


        }
    }
}