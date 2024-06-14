namespace AgendamentoAPI.Requests
{
    public class NivelAcessoRequest(string Acesso)
    {
        public string Acesso { get; } = Acesso;
    }
}
