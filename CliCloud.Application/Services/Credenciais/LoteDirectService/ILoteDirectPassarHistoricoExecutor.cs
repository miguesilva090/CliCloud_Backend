using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectPassarHistoricoExecutor : ITransientService
{
    Task<PassarParaHistoricoResultDTO> ExecutarAsync(
        int codigoOrganismo,
        int mes,
        int ano,
        CancellationToken cancellationToken = default);
}
