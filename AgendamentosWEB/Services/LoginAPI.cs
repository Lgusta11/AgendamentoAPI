using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using AgendamentosWEB.Requests;
using System.Net.Http.Headers;
using AgendamentosWEB.Response;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Authorization;

namespace AgendamentosWEB.Services
{
    public class LoginAPI : AuthenticationStateProvider
    {
        private bool autenticado = false;
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginAPI> _logger;
        private readonly IJSRuntime _jsRuntime;

        public LoginAPI(IHttpClientFactory factory, ILogger<LoginAPI> logger, IJSRuntime jsRuntime)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var pessoa = new ClaimsPrincipal();
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("auth/manage/info");

                if (response.IsSuccessStatusCode)
                {
                    var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                    if (info != null)
                    {
                        if (info.UserName != null && info.Email != null)
                        {
                            Claim[] dados =
                            {
                        new Claim(ClaimTypes.Name, info.UserName),
                        new Claim(ClaimTypes.Email, info.Email)
                    };

                            var identity = new ClaimsIdentity(dados, "AuthenticationType");
                            pessoa = new ClaimsPrincipal(identity);
                            autenticado = true;

                            // Armazenar o estado de autenticação no localStorage
                            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "autenticado", autenticado.ToString());
                        }
                        else
                        {
                            _logger.LogError("Erro ao recuperar informações do usuário");
                            return null!;
                        }
                    }
                }
            }

            return new AuthenticationState(pessoa);
        }


        public async Task<bool> VerificaAutenticado()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("auth/manage/info");

                if (response.IsSuccessStatusCode)
                {
                    var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                    if (info != null)
                    {
                        if (info.UserName != null && info.Email != null)
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogError("Erro ao recuperar informações do usuário");
                            return false;
                        }
                    }
                }
            }

            return false;
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
                    return new LoginResponse { Sucesso = false, Erros = new[] { "Login/senha inválidos" } };
                }

                var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (content == null || !content.ContainsKey("token"))
                {
                    _logger.LogError("Token não encontrado na resposta para o usuário {Email}", email);
                    return new LoginResponse { Sucesso = false, Erros = new[] { "Token não encontrado na resposta" } };
                }

                // Armazenar o token no localStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", content["token"]);

                // Adicionar o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content["token"]);

                // Verificar se o token foi adicionado corretamente
                if (_httpClient.DefaultRequestHeaders.Authorization?.Parameter != content["token"])
                {
                    _logger.LogError("Falha ao adicionar o token ao cabeçalho de autorização para o usuário {Email}", email);
                    return new LoginResponse { Sucesso = false, Erros = new[] { "Falha ao adicionar o token ao cabeçalho de autorização" } };
                }

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, email)
        }, "Cookies")))));
                _logger.LogInformation("Login bem sucedido para o usuário {Email}", email);
                return new LoginResponse { Sucesso = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o login para o usuário {Email}", email);
                return new LoginResponse { Sucesso = false, Erros = new[] { ex.Message } };
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
                    return responseObject != null ? responseObject["roles"] : new List<string>();
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

        public async Task LogoutAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "token");
                _httpClient.DefaultRequestHeaders.Authorization = null;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
                _logger.LogInformation("Logout bem sucedido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante o logout");
            }
        }


        public async Task<InfoPessoaResponse?> GetUserInfoAsync()
        {
            try
            {
                var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token");

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = await _httpClient.GetAsync("auth/manage/info");
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
                    }
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
    }
}