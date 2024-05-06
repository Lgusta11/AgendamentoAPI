using System.ComponentModel.DataAnnotations;

namespace AgendamentosWEB.Requests;
public class EquipamentosRequestEdit
{
    public int Id { get; set; }

    [Required]
    [StringLength(30, ErrorMessage = "O nome do equipamento não pode ter mais de 30 caracteres.")]
    public string? Nome { get; set; }

    [Required]
    public int Quantidade { get; set; }

    public EquipamentosRequestEdit() { }

    public EquipamentosRequestEdit(int id, string nome, int quantidade)
    {
        Id = id;
        Nome = nome;
        Quantidade = quantidade;
    }
}

