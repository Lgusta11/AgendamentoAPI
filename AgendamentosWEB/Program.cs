using AgendamentosWEB;
using AgendamentosWEB.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();

// Adicione os serviços necessários
builder.Services.AddTransient<AdminAPI>();
builder.Services.AddTransient<LoginAPI>();
builder.Services.AddTransient<AgendamentosAPI>();
builder.Services.AddTransient<CadastroAPI>();
builder.Services.AddTransient<EquipamentosAPI>();
builder.Services.AddTransient<ProfessoresAPI>();
builder.Services.AddTransient<AulasAPI>();

// Adicione o serviço de autenticação
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Adicione o provedor de autenticação
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));
builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
