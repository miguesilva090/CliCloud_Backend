using System.Globalization;
using CliCloud.Domain.Entities.Documentos;

namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService;

internal static class FicheiroEletronicoFormatHelper
{
    /// <summary>
    /// Equivalente legado a <c>TFatura.TotalFatura</c>
    /// (<c>TotalDocumento − RetencaoFonte</c>; relação <c>TotalFatura + Retencao = TotalDocumento</c>).
    /// </summary>
    public static decimal ResolverTotalFaturaLegado(Documento doc) =>
        (doc.TotalDocumento ?? 0m) - (doc.RetencaoValor ?? 0m);
    public static string Pad(string value, int length, char padChar = ' ')
        => value.Length >= length ? value[..length] : value + new string(padChar, length - value.Length);

    public static string PadLeft(string value, int length, char padChar = '0')
        => value.Length >= length ? value[^length..] : new string(padChar, length - value.Length) + value;

    public static string ValorCentimos(decimal valor, int digits = 11)
    {
        string raw = decimal.Round(valor, 2).ToString("F2", CultureInfo.InvariantCulture).Replace(".", "");
        return PadLeft(raw, digits);
    }

    public static string ObterFilial(string? sucursal)
    {
        if (string.IsNullOrWhiteSpace(sucursal)) return "00001";
        string t = sucursal.Trim();
        if (t.Length > 5) throw new InvalidOperationException("A filial deve ter no máximo 5 caracteres");
        return t.Length == 5 ? t : PadLeft(t, 5);
    }

    public static string ObterCodigoEntidadePsp(string? codigoClinica)
    {
        if (string.IsNullOrWhiteSpace(codigoClinica))
            throw new InvalidOperationException("Certifique-se que tem o Código da Clínica preenchido na ficha do organismo");
        
        string t = codigoClinica.Trim();
        return t.Length > 7 ? t[^7..] : PadLeft(t, 7);
    }
}