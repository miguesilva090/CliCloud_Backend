using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public interface IAdseComunicacaoListReader : ITransientService
{
    Task<AdseComunicacaoPaginatedDTO> ObterPaginadoAsync(
        Guid clinicaId,
        Guid organismoAdseId,
        string tipoPreFaturaLegado,
        AdseComunicacaoTableFilter filter,
        CancellationToken cancellationToken = default);
}
