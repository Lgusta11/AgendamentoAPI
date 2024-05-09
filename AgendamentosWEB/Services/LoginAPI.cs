using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using AgendamentosWEB.Requests;
using System.Net.Http.Headers;
using AgendamentosWEB.Response;

namespace AgendamentosWEB.Services
{
    public class LoginAPI : AuthenticationStateProvider
    {
        private bool autenticado = false;
        private readonly HttpClient _httpClient;

        public LoginAPI(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            autenticado = false;
            var pessoa = new ClaimsPrincipal();
            var response = await _httpClient.GetAsync("auth/manage/info");

            if (response.IsSuccessStatusCode)
            {
                var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                Claim[] dados =
                {
                    new Claim(ClaimTypes.Name, info?.Email!),
                    new Claim(ClaimTypes.Email, info?.Email!)
                };

                var identity = new ClaimsIdentity(dados, "Cookies");
                pessoa = new ClaimsPrincipal(identity);
                autenticado = true;
            }

            return new AuthenticationState(pessoa);
        }

        public async Task<LoginResponse> LoginAsync(string email, string senha)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/Login", new
            {
                email,
                password = senha
            });

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (content != null && content.ContainsKey("token"))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content["token"]);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new LoginResponse { Sucesso = true };
                }
            }

            return new LoginResponse { Sucesso = false, Erros = ["Login/senha inválidos"] };
        }

        public async Task<List<string>> GetUserRolesAsync(string email)
        {
            var response = await _httpClient.GetAsync($"auth/GetRoles/{email}");

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
                var roles = responseObject["roles"];
                return roles!;
            }

            return new List<string>();
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("auth/logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> VerificaAutenticado()
        {
            await GetAuthenticationStateAsync();
            return autenticado;
        }
    }
}