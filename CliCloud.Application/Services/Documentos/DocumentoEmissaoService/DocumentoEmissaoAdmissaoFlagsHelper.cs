using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

/// <summary>
/// Paridade legado <c>ADMISS.setAdmissaoFaturada</c> — recibo (FR) marca pago, não faturado global.
/// </summary>
internal static class DocumentoEmissaoAdmissaoFlagsHelper
{
    private const int SaftFaturaRecibo = 3;

    public static bool IsFaturaRecibo(TipoDocumento tipo) =>
        tipo.CodigoTipoDocumentoSaft == SaftFaturaRecibo
        || string.Equals(tipo.Abreviatura?.Trim(), "FR", StringComparison.OrdinalIgnoreCase);

    public static (bool Pago, bool Faturado) ResolverFlags(
        TipoDocumento tipoDocumento,
        bool? pagoRequest,
        bool? faturadoRequest)
    {
        if (IsFaturaRecibo(tipoDocumento))
        {
            return (pagoRequest ?? true, faturadoRequest ?? false);
        }

        return (pagoRequest ?? false, faturadoRequest ?? false);
    }
}
