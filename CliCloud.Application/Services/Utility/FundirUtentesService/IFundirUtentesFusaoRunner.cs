using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FundirUtentesService;

public interface IFundirUtentesFusaoRunner : IScopedService
{
    Task ExecutarFusaoReferenciasAsync(
        Guid utenteOrigemId,
        Guid utenteApagarId,
        CancellationToken cancellationToken = default);
}
