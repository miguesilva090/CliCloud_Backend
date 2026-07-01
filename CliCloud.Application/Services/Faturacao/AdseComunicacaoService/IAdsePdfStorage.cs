using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public interface IAdsePdfStorage : ITransientService
{
    Task<string> GuardarAsync(Guid clinicaId, string pastaRelativa, string nomeFicheiro, byte[] conteudo, CancellationToken ct = default);
    Task<byte[]?> LerAsync(Guid clinicaId, string pastaRelativa, string nomeFicheiro, CancellationToken ct = default);
}
