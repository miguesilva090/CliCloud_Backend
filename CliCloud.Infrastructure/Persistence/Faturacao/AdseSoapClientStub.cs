using CliCloud.Application.Services.Faturacao.AdseComunicacaoService;
using CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;
using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class AdseSoapClientStub : IAdseSoapClient
{
    public Task<string?> ExecutarDocumentoAsync(
        WebserviceAdse config, int operacaoLegado, AdseComunicacaoLinhaDTO linha,
        string codigoPreFatura, byte[]? pdf, byte[]? pdfRelatorio, CancellationToken ct = default)
    {
        if (operacaoLegado == AdseEstados.OperacaoValidar)
            return Task.FromResult<string?>(null);
        return Task.FromResult<string?>("Integração SOAP ADSE pendente (Fase F4).");
    }
}
