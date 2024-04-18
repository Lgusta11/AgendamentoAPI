namespace AgendamentoAPI.Response
{
    public record AgendamentoResponse(int Id, DateTime Data, int AulaId, int EquipamentoId, int ProfessorId);
}
