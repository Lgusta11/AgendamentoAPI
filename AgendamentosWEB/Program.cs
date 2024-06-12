using AgendamentosWEB;
using AgendamentosWEB.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Blazored.LocalStorage; // Adicione esta linha

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, LoginAPI>();
builder.Services.AddScoped<LoginAPI>(sp => (LoginAPI)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();
builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage(); // Adicione esta linha

// Adicione os serviços necessários
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddScoped<JwtAuthorizationMessageHandler>(); // Adicione esta linha

builder.Services.AddScoped<AdminAPI>();
builder.Services.AddScoped<LoginAPI>();
builder.Services.AddScoped<AgendamentosWEB.Services.AgendamentosAPI>();
builder.Services.AddScoped<CadastroAPI>();
builder.Services.AddScoped<EquipamentosAPI>();
builder.Services.AddScoped<ProfessoresAPI>();
builder.Services.AddScoped<AulasAPI>();

// Adicione o serviço de autenticação
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<CookieHandler>()
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>(); // Adicione esta linha

builder.Services.AddHttpClient("HolidayAPI", client => {
    client.BaseAddress = new Uri(builder.Configuration["HolidayAPI:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

await builder.Build().RunAsync();