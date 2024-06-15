namespace AgendamentoAPI.Response
{
    public record UserResponse(string Id,string UserName, string Email, string Senha, string? NivelAcesso);
   
}
