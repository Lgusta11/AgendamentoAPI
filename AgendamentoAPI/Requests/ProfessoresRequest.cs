using System.ComponentModel.DataAnnotations;

namespace Agendamentos.Requests
{
    public record ProfessoresRequest([Required] string nome,string? fotoPerfil);
}
