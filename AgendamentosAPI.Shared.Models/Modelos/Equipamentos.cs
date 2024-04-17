namespace Agendamentos.Shared.Modelos.Modelos
{
    public class Equipamentos
    {
        public Equipamentos(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
        public int Id { get; set; }
        public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

    }



   
}
