using AgendamentoAPI.Email;
using AgendamentoAPI.EndPoints;
using AgendamentoAPI.EndPoints.AdminCrud;
using Agendamentos.EndPoints;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configurações do DbContext, Identity, etc.
builder.Services.AddDbContext<AgendamentosContext>();
builder.Services.AddScoped<PessoaComAcesso>();
builder.Services.AddIdentity<PessoaComAcesso, Admin>()
    .AddEntityFrameworkStores<AgendamentosContext>()
    .AddDefaultTokenProviders();

// Configuração do CORS
builder.Services.AddCors(options => options.AddPolicy("wasm", policy => policy
    .WithOrigins(builder.Configuration["BackendUrl"] ?? "https://localhost:7054")
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowAnyHeader()
    .AllowCredentials()));

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do JSON Serializer
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Middleware para servir arquivos estáticos
app.UseStaticFiles();

// Configuração do JWT Authentication
var issuer = builder.Configuration["Jwt:Issuer"];
var key = builder.Configuration["Jwt:Key"];
if (issuer == null || key == null)
{
    throw new Exception("Jwt:Issuer and Jwt:Key must be defined in the configuration.");
}

app.UseAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Configuração do CORS
app.UseCors("wasm");

// Middleware de Autorização
app.UseAuthorization();

// Adição dos Endpoints
app.AddEndPointsProfessores();
app.AddEndPointsAulas();
app.AddEndPointsEquipamentos();
app.AddEndPointsAgendamentos();
app.AddEndPointsAdmin();
app.AddEndPointsCadastro();
app.AddEndPoinsLogin();

// Endpoint de Logout
app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).WithTags("Autenticação");

// Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Inicialização do aplicativo
app.Run();
