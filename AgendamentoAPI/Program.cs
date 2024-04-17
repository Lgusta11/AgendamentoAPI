using Agendamentos.EndPoints;
using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AgendamentosContext>();


builder.Services.AddIdentityApiEndpoints<PessoaComAcesso>()
    .AddEntityFrameworkStores<AgendamentosContext>();

builder.Services.AddAuthorization();

builder.Services.AddScoped<DAL<Professores>>();
//builder.Services.AddScoped<DAL<Equipamentos>>();
//builder.Services.AddScoped<DAL<Aulas>>();
//builder.Services.AddScoped<DAL<Agendamento>>();

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
//app.AddEndPointsAulas();
//app.AddEndPointsEquipamentos();
//app.AddEndPointsAgendamentos();


app.MapGroup("/auth").MapIdentityApi<PessoaComAcesso>()
    .WithTags("Autenticação");

app.UseSwagger();
app.UseSwaggerUI();



app.Run();

