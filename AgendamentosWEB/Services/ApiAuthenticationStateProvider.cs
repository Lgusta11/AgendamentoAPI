using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using AgendamentosWEB.Response;
using AgendamentosWEB.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class ApiAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public ApiAuthenticationStateProvider(IHttpClientFactory factory, ILocalStorageService localStorage)
    {
        _httpClient = factory.CreateClient("API");
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("AuthToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var response = await _httpClient.PostAsJsonAsync("/auth/AutenticadoInfo", new { token = savedToken });

        if (!response.IsSuccessStatusCode)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var userInfo = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userInfo!.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, userInfo.Email ?? string.Empty)
        };

        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        return new AuthenticationState(user);
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var claims = JwtParser.ParseClaimsFromJwt(token);
        var identity = new ClaimsIdentity(claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void MarkUserAsLoggedOut()
    {
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("AuthToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;
        MarkUserAsLoggedOut();
    }
}
