using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectPassarAtivoExecutor
{
    Task<PassarParaAtivoResultDTO> ExecutarAsync(
        int codigoOrganismo,
        int mesOrigem,
        int anoOrigem,
        int mesNovo,
        int anoNovo,
        CancellationToken cancellationToken = default
    );
}