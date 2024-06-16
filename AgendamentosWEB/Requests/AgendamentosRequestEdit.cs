namespace AgendamentosWEB.Requests
{
    public record AgendamentosRequestEdit(int Id, DateTime Data, ICollection<int> AulaIds, int EquipamentoId, string ProfessorId)
       : AgendamentoRequest(Data, AulaIds, EquipamentoId, ProfessorId);
}
