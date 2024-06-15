using AgendamentoAPI;
using AgendamentoAPI.Auth;
using AgendamentoAPI.EndPoints;
using Agendamentos.EndPoints;
using Agendamentos.Shared.Dados.Database;
using AgendamentosAPI.Shared.Dados;
using AgendamentosAPI.Shared.Dados.Database;
using AgendamentosAPI.Shared.Dados.Database.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure the DbContext with the connection string from appsettings.json

builder.Services.AddDbContext<AgendamentosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AgendamentosContext>();

builder.Services.AddHostedService<CleanupService>();

builder.Services.AddLogging();
builder.Services.AddScoped(typeof(DAL<>));
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProfessorService>();
builder.Services.AddScoped<NiveisdeAcessoService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SistemaAFS", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Configuração do JSON Serializer
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(
    options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAuthorization();

var key = Encoding.ASCII.GetBytes(Settings.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


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

/*

// Obtenha uma instância do RoleManager
using var serviceScope = app.Services.CreateScope();
var serviceProvider = serviceScope.ServiceProvider;

*/


// Adição dos Endpoints
app.AddEndPointsProfessores();
app.AddEndPointsAulas();
app.AddEndPointsEquipamentos();
app.AddEndPointsAgendamentos();
app.AddEndPointsCadastro();
app.AddEndPoinsLogin();
app.AddEndPoinsUsers();



// Configuração do Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Inicialização do aplicativo
app.Run();