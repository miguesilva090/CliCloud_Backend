using CliCloud.Application.Services.Faturacao.AdseComunicacaoService;
using Microsoft.Extensions.Hosting;

namespace CliCloud.Infrastructure.Persistence.Faturacao;

public sealed class AdsePdfFileStorage(IHostEnvironment env) : IAdsePdfStorage
{
    public async Task<string> GuardarAsync(
        Guid clinicaId, string pastaRelativa, string nomeFicheiro, byte[] conteudo, CancellationToken ct = default)
    {
        string dir = Path.Combine(
            env.ContentRootPath, "UserFiles", clinicaId.ToString("N"), pastaRelativa.Trim('\\', '/'));
        Directory.CreateDirectory(dir);
        string fullPath = Path.Combine(dir, Path.GetFileName(nomeFicheiro));
        await File.WriteAllBytesAsync(fullPath, conteudo, ct).ConfigureAwait(false);
        return Path.GetFileName(fullPath);
    }

    public async Task<byte[]?> LerAsync(
        Guid clinicaId, string pastaRelativa, string nomeFicheiro, CancellationToken ct = default)
    {
        string fullPath = Path.Combine(
            env.ContentRootPath, "UserFiles", clinicaId.ToString("N"),
            pastaRelativa.Trim('\\', '/'), Path.GetFileName(nomeFicheiro));
        return File.Exists(fullPath) ? await File.ReadAllBytesAsync(fullPath, ct).ConfigureAwait(false) : null;
    }
}
