using Agendamentos.Shared.Dados.Modelos;
using Agendamentos.Shared.Modelos.Modelos;
using AgendamentosAPI.Shared.Models.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Agendamentos.Shared.Dados.Database
{
    public class AgendamentosContext : IdentityDbContext<PessoaComAcesso, Admin, int>
    {
        public DbSet<Professores> Professores { get; set; }
        public DbSet<Equipamentos> Equipamentos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Aulas> Aulas { get; set; }
        public DbSet<AgendamentoAula> AgendamentoAulas { get; set; }

        private readonly IConfiguration _configuration;

        public AgendamentosContext(DbContextOptions<AgendamentosContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                optionsBuilder
                    .UseNpgsql(connectionString)
                    .UseLazyLoadingProxies()
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para Identity
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<IdentityUserLogin<int>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });

            modelBuilder.Entity<IdentityUserToken<int>>()
                .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

            // Configurações para entidades personalizadas
            modelBuilder.Entity<Professores>().HasKey(p => p.Id);
            modelBuilder.Entity<Equipamentos>().HasKey(e => e.Id);
            modelBuilder.Entity<Aulas>().HasKey(a => a.Id);
            modelBuilder.Entity<Agendamento>().HasKey(a => a.Id);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Professor)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.ProfessorId);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Equipamento)
                .WithMany(e => e.Agendamentos)
                .HasForeignKey(a => a.EquipamentoId);

            modelBuilder.Entity<AgendamentoAula>()
                .HasKey(aa => new { aa.AgendamentoId, aa.AulaId });

            modelBuilder.Entity<AgendamentoAula>()
                .HasOne(aa => aa.Agendamento)
                .WithMany(a => a.AgendamentoAulas)
                .HasForeignKey(aa => aa.AgendamentoId);

            modelBuilder.Entity<AgendamentoAula>()
                .HasOne(aa => aa.Aula)
                .WithMany(a => a.AgendamentoAulas)
                .HasForeignKey(aa => aa.AulaId);

            // Configuração para converter todos os DateTime para UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion(dateTimeConverter);
                }
            }
        }
    }
}
