using AgendamentoAPI.Email;
using AgendamentoAPI.EndPoints.AdminCrud;
using Agendamentos.EndPoints;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AgendamentosContext>();


builder.Services.AddIdentity<PessoaComAcesso, Admin>()
    .AddEntityFrameworkStores<AgendamentosContext>()
    .AddDefaultTokenProviders();


builder.Services.AddSingleton<IEmailSender<PessoaComAcesso>, DummyEmailSender>();

builder.Services.AddAuthorization();

builder.Services.AddScoped<DAL<Professores>>();
builder.Services.AddScoped<DAL<Equipamentos>>();
builder.Services.AddScoped<DAL<Aulas>>();
builder.Services.AddScoped<DAL<Agendamento>>();
builder.Services.AddScoped<DAL<Admin>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7054"])
            .AllowAnyMethod()
            .SetIsOriginAllowed(pol => true)
            .AllowAnyHeader()
            .AllowCredentials()));

var app = builder.Build();



app.UseCors("wasm");

app.UseStaticFiles();
app.UseAuthorization();

app.AddEndPointsProfessores();
app.AddEndPointsAulas();
app.AddEndPointsEquipamentos();
app.AddEndPointsAgendamentos();
app.AddEndPointsAdmin();


using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<Admin>>();
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var admin = new Admin { Name = "Admin", Nome = "Admin", Email = "admin@example.com" };
        await roleManager.CreateAsync(admin);
    }
}


app.MapGroup("/auth").MapIdentityApi<PessoaComAcesso>()
    .WithTags("Autenticação");

app.MapPost("auth/logout", async ([FromServices] SignInManager<PessoaComAcesso> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
}).RequireAuthorization().WithTags("Autorização");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
