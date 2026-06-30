using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectPassarHistoricoExecutor
{
    Task<PassarParaHistoricoResultDTO> ExecutarAsync(
        int codigoOrganismo,
        int mes,
        int ano,
        CancellationToken cancellationToken = default);
}
