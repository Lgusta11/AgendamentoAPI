using AgendamentosWEB.Requests;
using AgendamentosWEB.Response;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AgendamentosWEB.Services;
public class CadastroAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public CadastroAPI(IHttpClientFactory factory, ILocalStorageService localStorageService)
    {
        _httpClient = factory.CreateClient("API");
        _localStorageService = localStorageService;
    }

    public async Task<InfoPessoaResponse?> CadastroAdminAsync(UserRequest userRequest)
    {
        var savedtoken = await _localStorageService.GetItemAsync<string>("AuthToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", savedtoken);

        string acessoId = "db731824-8702-423b-bbed-97aa37ca9494";

        StringContent jsonBody = new(JsonSerializer.Serialize(new
        {
            userName = userRequest.UserName,
            email = userRequest.Email,
            password = userRequest.Senha,
            acessoId
        }),
        Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PostAsync("/auth/Cadastro/Usuarios", jsonBody);

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

        string acessoId = "2";

        StringContent jsonBody = new(JsonSerializer.Serialize(new
        {
            userName = userRequest.UserName,
            email = userRequest.Email,
            password = userRequest.Senha,
            acessoId
        }),
        Encoding.UTF8,
        "application/json");

        var response = await _httpClient.PostAsync("/auth/Cadastro/Usuarios", jsonBody);

        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();
    }
}

