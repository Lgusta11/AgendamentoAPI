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
    .AddRoleManager<RoleManager<Admin>>()
    .AddDefaultTokenProviders();

builder.Services.AddHostedService<CleanupService>();

builder.Services.AddLogging();
builder.Services.AddScoped<DAL<Aulas>>();
builder.Services.AddScoped<DAL<Equipamentos>>();
builder.Services.AddScoped<DAL<Professores>>();
builder.Services.AddScoped<DAL<Agendamento>>();
builder.Services.AddScoped<DAL<Admin>>();


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
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7054",
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7056"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));


var app = builder.Build();

app.UseCors("wasm");

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
