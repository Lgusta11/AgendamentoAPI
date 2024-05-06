using AgendamentosWEB;
using AgendamentosWEB.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, LoginAPI>();
builder.Services.AddScoped(sp => (LoginAPI)sp.GetRequiredService<AuthenticationStateProvider>());


builder.Services.AddMudServices();


builder.Services.AddScoped<CookieHandler>();
// Adicione os servi�os necess�rios
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<AdminAPI>();
builder.Services.AddScoped<LoginAPI>();
builder.Services.AddScoped<AgendamentosAPI>();
builder.Services.AddScoped<CadastroAPI>();
builder.Services.AddScoped<EquipamentosAPI>();
builder.Services.AddScoped<ProfessoresAPI>();
builder.Services.AddScoped<AulasAPI>();

// Adicione o servi�o de autentica��o
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<CookieHandler>();


await builder.Build().RunAsync();
