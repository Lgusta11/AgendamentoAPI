namespace Agendamentos.Requests
{
    public record ProfessoresRequestEdit(int Id, string nome, string? fotoPerfil)
      : ProfessoresRequest(nome, fotoPerfil);
}
