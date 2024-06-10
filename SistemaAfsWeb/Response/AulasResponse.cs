namespace SistemaAfsWeb.Response
{
    public record AulasResponse(int Id, string Aula, TimeSpan Duracao)
    {
        public override string ToString()
        {
            return $"{Aula}";
        }
    }
}
