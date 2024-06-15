using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace AgendamentosWEB.Services;
public class CadastroAPI
    {
        private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public CadastroAPI(IHttpClientFactory factory, ILocalStorageService localStorageService)
        {
            _httpClient = factory.CreateClient("API");
        }

    public async Task<InfoPessoaResponse?> CadastroAdminAsync(UserRequest userRequest)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        userRequest.AcessoId = "7334c9b6-fda8-4e99-9c61-bfcb272483c7";

        var response = await _httpClient.PostAsJsonAsync("auth/Cadastro/Usuarios", userRequest);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
        }
        return null;
    }


    public async Task<InfoPessoaResponse?> CadastroProfessorAsync(UserRequest userRequest)
        {

        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        userRequest.AcessoId = "de69e3df-04f3-44c6-b720-bbddceb476e5";

        var response = await _httpClient.PostAsJsonAsync("auth/Cadastro/Usuarios", userRequest);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
        }
        return null;
    }
    }

