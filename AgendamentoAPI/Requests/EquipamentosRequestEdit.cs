namespace AgendamentoAPI.Requests
{
    public record EquipamentosRequestEdit(int Id, string Nome, int Quantidade)
          : EquipamentoRequest(Nome, Quantidade);
}
