using Agendamentos.Shared.Dados.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgendamentoAPI.EndPoints
{
    public static class LoginExtensions
    {
        public static void AddEndPoinsLogin(this WebApplication app)
        {
            app.MapPost("auth/login", async ([FromServices] UserManager<PessoaComAcesso> userManager, [FromBody] LoginRequest loginRequest, IConfiguration _config) =>
            {
                var user = await userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null || !(await userManager.CheckPasswordAsync(user, loginRequest.Password)))
                {
                    return Results.Unauthorized(new { Message = "Email ou senha incorretos." });
                }

                // Geração do Token JWT
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var userRoles = await userManager.GetRolesAsync(user);
                claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var jwtKey = _config["Jwt:Key"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Results.Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }).WithTags("Autenticação");
        }
    }
}
