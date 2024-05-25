namespace AgendamentoAPI.Requests
{
    public record AgendamentosRequestEdit(int Id, DateTime Data, ICollection<int> AulaIds, int EquipamentoId, int ProfessorId)
       : AgendamentoRequest(Data, AulaIds, EquipamentoId, ProfessorId);
}
