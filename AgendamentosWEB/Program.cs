using AgendamentosWEB;
using AgendamentosWEB.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Blazored.LocalStorage;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, LoginAPI>();
builder.Services.AddScoped<LoginAPI>(sp => (LoginAPI)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();
builder.Services.AddRadzenComponents();
builder.Services.AddBlazoredLocalStorage(); 
builder.Services.AddScoped<JwtAuthorizationMessageHandler>();



// Adicione o servi�o de autentica��o
builder.Services.AddHttpClient("API", client => {
    client.BaseAddress = new Uri(builder.Configuration["APIServer:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>(); 

builder.Services.AddHttpClient("HolidayAPI", client => {
    client.BaseAddress = new Uri(builder.Configuration["HolidayAPI:Url"]!);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

await builder.Build().RunAsync();
