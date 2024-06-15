using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace AgendamentosWEB.Services
{
    public class LoginAPI
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginAPI> _logger;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public LoginAPI(IHttpClientFactory factory, ILogger<LoginAPI> logger, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }
        /*
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            autenticado = false;
            var pessoa = new ClaimsPrincipal();
            var response = await _httpClient.GetAsync("auth/autenticado");

            if (response.IsSuccessStatusCode)
            {
                var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                Claim[] dados =
                [
                    new Claim(ClaimTypes.Name, info.UserName),
                new Claim(ClaimTypes.Email, info.Email)
                ];

                var identity = new ClaimsIdentity(dados, "Cookies");
                pessoa = new ClaimsPrincipal(identity);
                autenticado = true;
            }

            return new AuthenticationState(pessoa);
        }
        */

        public async Task<bool> LoginAsync(string email, string senha)
        {
            StringContent crendetials = new(JsonSerializer.Serialize(new
            {
                email,
                senha
            }),
            Encoding.UTF8,
            "application/json");

            var request = await _httpClient.PostAsync("auth/Login", crendetials);

            if (!request.IsSuccessStatusCode)
                return false;

            var response = await request.Content.ReadFromJsonAsync<AuthResponse>();

            await _localStorageService.SetItemAsync("AuthToken", response!.Token);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(response.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);

            return true;
        }

        //public async Task LogoutAsync()
        //{
        //    await _localStorageService.RemoveItemAsync("authToken");
        //    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        //    _httpClient.DefaultRequestHeaders.Authorization = null;
        //}

        /*
        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("auth/logout", null);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        */
        /*
        public async Task<bool> VerificaAutenticado()
        {
            await GetAuthenticationStateAsync();
            return autenticado;
        }
        */
        
        public async Task<InfoPessoaResponse> GetUserRolesAsync()
        {
            try
            {
                var savedToken = await _localStorageService.GetItemAsync<string>("AuthToken");

                StringContent bodyJson = new(JsonSerializer.Serialize(new
                {
                    token = savedToken,
                }),
                Encoding.UTF8,
                "application/json");

                var requestInfo = await _httpClient.PostAsync("/auth/AutenticadoInfo", bodyJson);

                if (!requestInfo.IsSuccessStatusCode)
                    throw new Exception("Erro ao recuperar informações");

                var response = await requestInfo.Content.ReadFromJsonAsync<InfoPessoaResponse>();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar as funções para o usuário");
                return new InfoPessoaResponse();
            }
        }

        /*
        public async Task<string?> GetUserNameAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"auth/GetUserName/{email}");

                if (response.IsSuccessStatusCode)
                {
                    var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    return responseObject?["userName"];
                }

                _logger.LogWarning("Não foi possível recuperar o nome de usuário para o email {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar o nome de usuário para o email {Email}", email);
                return null;
            }
        }
        */
        /*
        public async Task<InfoPessoaResponse?> GetUserInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("auth/manage/info");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                }

                _logger.LogError("Erro ao recuperar informações do usuário");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar informações do usuário");
                return null;
            }
        }
        */
    }
}