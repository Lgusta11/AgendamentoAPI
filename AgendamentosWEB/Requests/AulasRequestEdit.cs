namespace AgendamentosWEB.Requests
{
    public record AulasRequestEdit(int Id, string nome, TimeSpan Duracao) : AulasRequest(nome, Duracao);
}
