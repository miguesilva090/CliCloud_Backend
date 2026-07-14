using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectPassarAtivoExecutor : ITransientService
{
    Task<PassarParaAtivoResultDTO> ExecutarAsync(
        Guid loteDirectId,
        int mesNovo,
        int anoNovo,
        CancellationToken cancellationToken = default);
}
