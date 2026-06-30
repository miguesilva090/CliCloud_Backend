using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectCorrecaoLotesExecutor
{
    Task<CorrigirLotesResultDTO> ExecutarAsync(int ano, int mes, CancellationToken cancellationToken = default);
}