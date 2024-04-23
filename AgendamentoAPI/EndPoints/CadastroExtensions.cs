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

        private static string GenerateJwtToken(string email, IdentityUser user, IConfiguration _config)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            var jwtKey = _config["JwtKey"];
            if (jwtKey == null)
            {
                throw new ArgumentNullException(nameof(jwtKey), "JwtKey cannot be null.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _config["JwtIssuer"],
                _config["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public static void AddEndPointsCadastro(this WebApplication app)
        {
            var groupBuilder = app.MapGroup("auth")
                .WithTags("Autenticação");

            //ADMIN
            groupBuilder.MapPost("Cadastro/Admin", async ([FromServices] DAL<Admin> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromBody] AdminRequest adminRequest , IConfiguration _config) =>
            {
                var user = new PessoaComAcesso { UserName = adminRequest.Email, Email = adminRequest.Email };
                var result = await userManager.CreateAsync(user, adminRequest.Senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                await userManager.AddToRoleAsync(user, "Admin");

                var admin = new Admin { Nome = adminRequest.Nome, Email = adminRequest.Email };
                dal.Adicionar(admin);

                // Geração do Token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _config["Jwt:Key"];
                if (jwtKey == null)
                {
                    throw new ArgumentNullException(nameof(jwtKey), "JwtKey cannot be null.");
                }

                var key = Encoding.ASCII.GetBytes(jwtKey);

                if (key == null || key.Length < 16)
                {
                    return Results.BadRequest("A chave secreta deve ter pelo menos 16 caracteres.");
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
            new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Results.Ok(new { Token = tokenString });
            }).RequireAuthorization(new AuthorizeAttribute() { Roles = "Admin" });



            //PROFESSOR
            groupBuilder.MapPost("Cadastro/Professor", async ([FromServices] IHostEnvironment env, [FromServices] DAL<Professores> dal, [FromServices] UserManager<PessoaComAcesso> userManager, [FromBody] ProfessoresRequest professoresRequest, IConfiguration  _config) =>
            {
                if (professoresRequest.senha != professoresRequest.confirmacaoSenha)
                {
                    return Results.BadRequest("A senha e a confirmação de senha não correspondem.");
                }

                var user = new PessoaComAcesso { UserName = professoresRequest.email, Email = professoresRequest.email };
                var result = await userManager.CreateAsync(user, professoresRequest.senha);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors.Select(x => x.Description));
                }

                var nome = professoresRequest.nome.Trim();
                var imagemProfessor = DateTime.Now.ToString("ddMMyyyyhhss") + "." + nome + ".jpg";

                var path = Path.Combine(env.ContentRootPath,
                    "wwwroot", "FotosPerfil", imagemProfessor);

                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(professoresRequest.fotoPerfil!));
                using FileStream fs = new(path, FileMode.Create);
                await ms.CopyToAsync(fs);

                var Professores = new Professores(professoresRequest.nome) { FotoPerfil = $"/FotosPerfil/{imagemProfessor}" };

                dal.Adicionar(Professores);

                // Geração do Token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _config["Jwt:Key"];
                if (jwtKey == null)
                {
                    throw new ArgumentNullException(nameof(jwtKey), "JwtKey cannot be null.");
                }

                var key = Encoding.ASCII.GetBytes(jwtKey);

                if (key == null || key.Length < 16)
                {
                    return Results.BadRequest("A chave secreta deve ter pelo menos 16 caracteres.");
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
            new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Results.Ok(new { Token = tokenString });
            });

        }
    }
}