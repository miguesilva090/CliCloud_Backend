using CliCloud.Application.Common.Marker;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public interface IAdseSoapClient : ITransientService
{
    Task<string?> ExecutarDocumentoAsync(
        WebserviceAdse config,
        int operacaoLegado,
        AdseComunicacaoLinhaDTO linha,
        string codigoPreFatura,
        byte[]? pdf,
        byte[]? pdfRelatorio,
        CancellationToken ct = default);
}
