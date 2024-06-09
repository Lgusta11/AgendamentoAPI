namespace AgendamentosWEB.Response
{
    public record AgendamentoResponse(int Id, DateTime Data, string NomeAula, string NomeEquipamento, string ProfessorNome);
}
