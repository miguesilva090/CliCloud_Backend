using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService;

public interface ILoteDirectEspHistoricoGateway : ITransientService
{
    Task UpdateParaHistoricoAsync(string numeroRequisicao, CancellationToken cancellationToken = default);

    Task UpdateParaAtivoAsync(string numeroRequisicao, CancellationToken cancellationToken = default);

    Task UpdateMedicoAsync(string numeroRequisicao, string codigoMedico, CancellationToken cancellationToken = default);

    Task<LoteDirectEspRequisicaoData?> ObterPorCredencialAsync(
        string credencial,
        CancellationToken cancellationToken = default
    );

    Task<string?> ResolverCservinstAsync(
        int codigoOrganismo,
        string codigoServico,
        CancellationToken cancellationToken = default
    );
}
