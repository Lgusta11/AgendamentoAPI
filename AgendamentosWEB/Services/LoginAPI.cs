﻿using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;

namespace AgendamentosWEB.Services
{
    public class LoginAPI : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginAPI> _logger;
        private readonly ILocalStorageService _localStorageService;
        private bool autenticado = false;

        public LoginAPI(IHttpClientFactory factory, ILogger<LoginAPI> logger, ILocalStorageService localStorageService)
        {
            _httpClient = factory.CreateClient("API");
            _logger = logger;
            _localStorageService = localStorageService;
        }

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
                    new Claim(ClaimTypes.Name, info.Email),
                new Claim(ClaimTypes.Email, info.Email)
                ];

                var identity = new ClaimsIdentity(dados, "Cookies");
                pessoa = new ClaimsPrincipal(identity);
                autenticado = true;
            }

            return new AuthenticationState(pessoa);
        }

        public async Task<bool> VerificaAutenticado()
          {
              await GetAuthenticationStateAsync();
              return autenticado;
          } 

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new { Email = email, Password = password });
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                {
                    await _localStorageService.SetItemAsync("authToken", loginResponse.Token);
                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new LoginResponse { Sucesso = true };
                }
            }

            return new LoginResponse { Sucesso = false };
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync("authToken");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
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
    }
}