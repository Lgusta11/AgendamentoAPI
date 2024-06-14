namespace AgendamentoAPI.Auth
{
    public interface ITokenService
    {
        Task<string> GetToken(string email, string senha);
    }
}
