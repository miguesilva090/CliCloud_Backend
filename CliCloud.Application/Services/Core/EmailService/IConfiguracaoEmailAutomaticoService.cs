using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.EmailService;

public interface IConfiguracaoEmailAutomaticoService : ITransientService
{
    Task ExecutarAsync(CancellationToken cancellationToken = default);
}
