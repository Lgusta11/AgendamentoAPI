using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using AgendamentosWEB.Requests;
using System.Net.Http.Headers;
using AgendamentosWEB.Response;
using Microsoft.Extensions.Logging;

namespace AgendamentosWEB.Services
{
    public class LoginAPI : AuthenticationStateProvider
    {
        private bool autenticado = false;
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginAPI> _logger;

        public LoginAPI(IHttpClientFactory factory, ILogger<LoginAPI> logger)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
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
                    new Claim(ClaimTypes.Name, info?.UserName!),
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
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth/Login", new
                {
                    email,
                    password = senha
                });

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Falha no login para o usuário {Email}", email);
                    return new LoginResponse { Sucesso = false, Erros = ["Login/senha inválidos"] };
                }

                var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (content == null || !content.ContainsKey("token"))
                {
                    _logger.LogError("Token não encontrado na resposta para o usuário {Email}", email);
                    return new LoginResponse { Sucesso = false, Erros = ["Token não encontrado na resposta"] };
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content["token"]);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                _logger.LogInformation("Login bem sucedido para o usuário {Email}", email);
                return new LoginResponse { Sucesso = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para o usuário {Email}", email);
                return new LoginResponse { Sucesso = false, Erros = [ex.Message] };
            }
        }

        public async Task<List<string>> GetUserRolesAsync(string emailOrUserName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"auth/GetRoles/{emailOrUserName}");

                if (response.IsSuccessStatusCode)
                {
                    var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>();
                    var roles = responseObject["roles"];
                    return roles!;
                }

                _logger.LogWarning("Não foi possível recuperar as funções para o usuário {EmailOrUserName}", emailOrUserName);
                return new List<string>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar as funções para o usuário {EmailOrUserName}", emailOrUserName);
                return new List<string>();
            }
        }

        public async Task<string> GetUserNameAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"auth/GetUserName/{email}");

                if (response.IsSuccessStatusCode)
                {
                    var responseObject = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    var userName = responseObject["userName"];
                    return userName;
                }

                _logger.LogWarning("Não foi possível recuperar o nome do usuário {Email}", email);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar o nome do usuário {Email}", email);
                return null;
            }
        }


        public async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync("auth/logout", null);
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                _logger.LogInformation("Logout bem sucedido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o logout");
            }
        }

        public async Task<bool> VerificaAutenticado()
        {
            await GetAuthenticationStateAsync();
            return autenticado;
        }
    }
}
