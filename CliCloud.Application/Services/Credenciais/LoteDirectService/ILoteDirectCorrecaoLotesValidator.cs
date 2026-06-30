using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectCorrecaoLotesValidator
{
    Task<ValidarCorrigirLotesDTO> ValidarAsync(int ano, int mes, CancellationToken cancellationToken = default);
}
