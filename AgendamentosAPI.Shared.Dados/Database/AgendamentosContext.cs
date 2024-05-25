using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agendamentos.Shared.Dados.Database
{
    public class AgendamentosContext : IdentityDbContext<PessoaComAcesso, Admin, int>
    {
        public DbSet<Professores> Professores { get; set; }
        public DbSet<Equipamentos> Equipamentos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Aulas> Aulas { get; set; }
        public DbSet<AgendamentoAula> AgendamentoAulas { get; set; }

        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Agendamentos;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(connectionString)
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Admin>()
            .HasIndex(a => a.Email)
            .IsUnique();
            // Configuração para a entidade IdentityUserRole<int>
            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
        

        // Configuração para a entidade IdentityUserLogin<int>
        modelBuilder.Entity<IdentityUserLogin<int>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Configuração para a entidade IdentityUserToken<int>
            modelBuilder.Entity<IdentityUserToken<int>>()
                .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            // Configuração para a entidade Professores
            modelBuilder.Entity<Professores>()
                .HasKey(p => p.Id); // Define a propriedade Id como chave primária

            // Configuração para a entidade Equipamentos
            modelBuilder.Entity<Equipamentos>()
                .HasKey(e => e.Id); // Define a propriedade Id como chave primária

            // Configuração para a entidade Aulas
            modelBuilder.Entity<Aulas>()
                .HasKey(a => a.Id); // Define a propriedade Id como chave primária

            // Configuração para a entidade Agendamentos
            modelBuilder.Entity<Agendamento>()
                .HasKey(a => a.Id); // Define a propriedade Id como chave primária

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Professor) // Define a relação com a entidade Professores
                .WithMany(p => p.Agendamentos) // Define a relação com a coleção de Agendamentos
                .HasForeignKey(a => a.ProfessorId); // Define a propriedade ProfessorId como chave estrangeira

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Equipamento) // Define a relação com a entidade Equipamentos
                .WithMany(e => e.Agendamentos) // Define a relação com a coleção de Agendamentos
                .HasForeignKey(a => a.EquipamentoId); // Define a propriedade EquipamentoId como chave estrangeira

            modelBuilder.Entity<AgendamentoAula>()
            .HasKey(aa => new { aa.AgendamentoId, aa.AulaId }); // Define a chave primária

            modelBuilder.Entity<AgendamentoAula>()
                .HasOne(aa => aa.Agendamento)
                .WithMany(a => a.AgendamentoAulas)
                .HasForeignKey(aa => aa.AgendamentoId);

            modelBuilder.Entity<AgendamentoAula>()
                .HasOne(aa => aa.Aula)
                .WithMany(a => a.AgendamentoAulas)
                .HasForeignKey(aa => aa.AulaId);



        }

    }
}
