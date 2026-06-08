using CliCloud.Domain.Entities.Consultas;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Servicos;

/// <summary>
/// Código curto do serviço para ficheiros eletrónicos (ex.: SAD/GNR, máx. 10 chars).
/// Usa apenas dados já existentes: AdmissaoServico.CodigoArtigo, Servico.EAN, DocumentoLinha.CodigoArtigo.
/// </summary>
internal static class CodigoServicoOrganismoHelper
{
    public const int MaxComprimentoFicheiroEletronico = 10;

    public static string Resolver(AdmissaoServico srv, DocumentoLinha? linha = null)
    {
        string? codigo = ResolverOpcional(srv, linha);
        if (codigo != null)
            return codigo;

        throw new InvalidOperationException(
            "Configure o código do serviço na admissão (CodigoArtigo) ou na ficha do serviço (EAN), "
            + $"com no máximo {MaxComprimentoFicheiroEletronico} caracteres.");
    }

    public static string? ResolverOpcional(AdmissaoServico srv, DocumentoLinha? linha = null)
    {
        string? designacao = srv.Servico?.Designacao ?? srv.NomeArtigo;

        foreach (string? candidato in Candidatos(srv, linha))
        {
            if (EhCodigoFicheiroEletronicoValido(candidato, designacao, linha?.Descricao))
                return candidato!.Trim();
        }

        return null;
    }

    private static IEnumerable<string?> Candidatos(AdmissaoServico srv, DocumentoLinha? linha)
    {
        yield return srv.CodigoArtigo;
        yield return srv.Servico?.EAN;
        yield return linha?.CodigoArtigo;
    }

    internal static bool EhCodigoFicheiroEletronicoValido(
        string? codigo,
        string? designacaoServicoExcluir,
        string? descricaoLinhaExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return false;

        string t = codigo.Trim();
        if (t.Length > MaxComprimentoFicheiroEletronico)
            return false;

        if (!string.IsNullOrWhiteSpace(designacaoServicoExcluir)
            && string.Equals(t, designacaoServicoExcluir.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(descricaoLinhaExcluir)
            && string.Equals(t, descricaoLinhaExcluir.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return true;
    }
}
