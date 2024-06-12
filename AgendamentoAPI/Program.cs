using AgendamentoAPI.EndPoints;
using AgendamentoAPI.EndPoints.AdminCrud;
using Agendamentos.EndPoints;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Dados.Database;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<AgendamentosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PessoaComAcesso>();
builder.Services.AddIdentity<PessoaComAcesso, Admin>()
    .AddEntityFrameworkStores<AgendamentosContext>()
    .AddRoleManager<RoleManager<Admin>>()
    .AddDefaultTokenProviders();

builder.Services.AddHostedService<CleanupService>();

builder.Services.AddLogging();
builder.Services.AddScoped(typeof(DAL<>));
builder.Services.AddScoped<AgendamentoService>();

// Configuração do JWT Authentication
var issuer = builder.Configuration["Jwt:Issuer"];
var key = builder.Configuration["Jwt:Key"];
if (issuer == null || key == null)
{
    throw new Exception("Jwt:Issuer and Jwt:Key must be defined in the configuration.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configurações do usuário
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;

    // Configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do JSON Serializer
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAuthorization();

// Configuração do CORS
builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
            policy => policy.WithOrigins(builder.Configuration["BackendUrl"] ?? "https://docker-aspnet-afs.onrender.com",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7056")
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

var app = builder.Build();

app.UseCors("wasm");

// Configuração de Middleware
app.UseAuthentication();
app.UseAuthorization();

// Obtenha uma instância do RoleManager
using var serviceScope = app.Services.CreateScope();
var serviceProvider = serviceScope.ServiceProvider;
var roleManager = serviceProvider.GetRequiredService<RoleManager<Admin>>();

// Verifique se a função "Professores" existe, se não, crie-a
if (!roleManager.RoleExistsAsync("Professores").Result)
{
    var roleResult = roleManager.CreateAsync(new Admin { Name = "Professores" }).Result;
}

// Adição dos Endpoints
app.AddEndPointsProfessores();
app.AddEndPointsAulas();
app.AddEndPointsEquipamentos();
app.AddEndPointsAgendamentos();
app.AddEndPointsAdmin();
app.AddEndPointsCadastro();
app.AddEndPoinsLogin();

// Endpoint de Logout
app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager, [FromServices] IHttpContextAccessor httpContextAccessor) =>
{
    await signInManager.SignOutAsync();
    httpContextAccessor.HttpContext?.Response.Cookies.Delete(".AspNetCore.Identity.Application");
    return Results.Ok(new { message = "Logout successful" });
}).WithTags("Autenticação");

// Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Inicialização do aplicativo
app.Run();