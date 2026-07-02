using CliCloud.Application.Common.Marker;
using CliCloud.Application.Common.Wrapper;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.Filters;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public interface IAdseComunicacaoService : ITransientService
{
    Task<Response<AdseComunicacaoPaginatedDTO>> GetPaginatedAsync(string modulo, AdseComunicacaoTableFilter filter, CancellationToken ct = default);
    Task<Response<IReadOnlyList<AdsePreFaturaDTO>>> ListarPreFaturasAbertasAsync(string tipoPreFatura, CancellationToken ct = default);
    Task<Response<IReadOnlyList<AdsePreFaturaDTO>>> ListarPreFaturasPorEstadoAsync(string tipoPreFatura, int estado, CancellationToken ct = default);
    Task<Response<Guid>> CriarPreFaturaAsync(CriarAdsePreFaturaRequest request, CancellationToken ct = default);
    Task<Response<bool>> ApagarPreFaturaAsync(Guid id, CancellationToken ct = default);
    Task<Response<AdsePreFaturaDTO>> ConferirPreFaturaAsync(Guid id, CancellationToken ct = default);
    Task<Response<AdsePreFaturaDTO>> ConsultarPreFaturaAsync(Guid id, CancellationToken ct = default);
    Task<Response<AdsePreFaturaDTO>> FecharPreFaturaAsync(Guid id, AdseFecharPreFaturaRequest request, CancellationToken ct = default);
    Task<Response<Guid>> RegistarPdfAsync(AdseUploadPdfRequest request, string tipoPreFatura, CancellationToken ct = default);
    Task<Response<string>> ComunicarDocumentosAsync(AdseComunicarDocumentosRequest request, CancellationToken ct = default);
    Task<Response<string>> LibertarDocumentosAsync(AdseLibertarDocumentosRequest request, CancellationToken ct = default);
}
