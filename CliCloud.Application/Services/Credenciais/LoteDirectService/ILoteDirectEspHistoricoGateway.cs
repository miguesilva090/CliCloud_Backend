using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectEspHistoricoGateway : ITransientService
{
    Task UpdateParaHistoricoAsync(string numeroRequisicao, CancellationToken cancellationToken = default);

    Task UpdateParaAtivoAsync(string numeroRequisicao, CancellationToken cancellationToken = default);
}
