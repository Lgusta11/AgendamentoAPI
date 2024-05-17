using Agendamentos.Shared.Dados.Database;
using Agendamentos.Shared.Modelos.Modelos;

public class CleanupService : BackgroundService
{
    private readonly IServiceProvider _services;

    public CleanupService(IServiceProvider services)
    {
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var dal = scope.ServiceProvider.GetRequiredService<DAL<Agendamento>>();
                var agendamentosPassados = dal.Listar(a => a.Data < DateTime.Today);
                foreach (var agendamento in agendamentosPassados)
                {
                    dal.Deletar(agendamento);
                }
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Executa a limpeza uma vez por dia
        }
    }
}
