using CliCloud.Application.Services.Core.SmsService;
using Microsoft.Extensions.Options;

namespace CliCloud.WebApi.HostedServices;

public class SmsAutomaticoHostedService (
    IServiceProvider serviceProvider,
    IOptions<SmsAutomaticoOptions> options, 
    ILogger<SmsAutomaticoHostedService> logger

) : BackgroundService 
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly SmsAutomaticoOptions _options = options.Value;
    private readonly ILogger<SmsAutomaticoHostedService> _logger = logger;



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if(!_options.Ativo)
        {
            _logger.LogInformation("SMS automático desativado por configuração");
            return;
        }

        if(!TimeSpan.TryParse(_options.HoraExecucao, out var horaExecucao))
            horaExecucao = new TimeSpan(11,5,0);

        while(!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var next = now.Date.Add(horaExecucao);
            if(next <= now) next = next.AddDays(1);

            await Task.Delay(next - now, stoppingToken);

            using var scope = _serviceProvider.CreateScope();
            var servico = scope.ServiceProvider.GetRequiredService<IServicoSmsAutomatico>();

            try
            {
                await servico.ExecutarAsync(stoppingToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erro no ciclo de envio de SMS automático.");
            }
        }
    }
}