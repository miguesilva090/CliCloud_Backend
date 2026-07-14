using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectCorrecaoLotesExecutor : ITransientService
{
    Task<CorrigirLotesResultDTO> ExecutarAsync(int ano, int mes, CancellationToken cancellationToken = default);
}