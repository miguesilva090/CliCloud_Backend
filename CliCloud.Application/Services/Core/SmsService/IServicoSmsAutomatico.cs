using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Core.SmsService;

public interface IServicoSmsAutomatico : ITransientService
{
    Task ExecutarAsync(CancellationToken cancellationToken = default);
}