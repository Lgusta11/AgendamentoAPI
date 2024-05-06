namespace AgendamentosWEB.Requests
{
    public record AgendamentosRequestEdit(int Id, DateTime Data, int AulaId, int EquipamentoId, int ProfessorId)
       : AgendamentoRequest(Data, AulaId, EquipamentoId, ProfessorId);
}
