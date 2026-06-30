using CliCloud.Domain.Entities.Utility;

namespace CliCloud.Application.Services.Documentos.DocumentoEmissaoService;

/// <summary>
/// Normaliza campos de snapshot fiscal para os limites da tabela <c>Documentos.Documento</c>.
/// </summary>
internal static class DocumentoEmissaoSnapshotHelper
{
    public const int NomeClienteMax = 100;
    public const int MoradaClienteMax = 100;
    public const int LocalidadeClienteMax = 40;
    public const int NumeroContribuinteClienteMax = 20;
    public const int BeneficiarioMax = 100;
    public const int CodigoArtigoMax = 50;
    public const int DescricaoLinhaMax = 250;

    public static string? TruncateOptional(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string trimmed = value.Trim();
        return trimmed.Length <= maxLength ? trimmed : trimmed[..maxLength];
    }

    public static string TruncateRequired(string? value, int maxLength, string fallback)
    {
        string? truncated = TruncateOptional(value, maxLength);
        return string.IsNullOrWhiteSpace(truncated) ? fallback : truncated;
    }

    public static string FormatarMoradaEntidade(Entidade? entidade)
    {
        if (entidade == null)
            return string.Empty;

        List<string> partes = [];
        if (!string.IsNullOrWhiteSpace(entidade.Rua?.Nome))
            partes.Add(entidade.Rua.Nome.Trim());
        if (!string.IsNullOrWhiteSpace(entidade.NumeroPorta))
            partes.Add(entidade.NumeroPorta.Trim());
        if (!string.IsNullOrWhiteSpace(entidade.AndarRua))
            partes.Add(entidade.AndarRua.Trim());

        return partes.Count > 0 ? string.Join(", ", partes) : string.Empty;
    }

    public static string ResolverMoradaCliente(string? requestMorada, Entidade? entidade)
    {
        if (!string.IsNullOrWhiteSpace(requestMorada))
            return TruncateRequired(requestMorada, MoradaClienteMax, "Morada não definida");

        string moradaEntidade = FormatarMoradaEntidade(entidade);
        if (!string.IsNullOrWhiteSpace(moradaEntidade))
            return TruncateRequired(moradaEntidade, MoradaClienteMax, "Morada não definida");

        return "Morada não definida";
    }

    public static string ResolverNomeCliente(string? requestNome, Entidade? entidadePrimaria, Entidade? entidadeSecundaria = null)
    {
        string? nome = requestNome
            ?? entidadePrimaria?.Nome
            ?? entidadeSecundaria?.Nome;

        return TruncateRequired(nome, NomeClienteMax, "Cliente sem nome");
    }

    public static string? ResolverLocalidadeCliente(string? requestLocalidade, Entidade? entidade)
    {
        string? localidade = requestLocalidade
            ?? entidade?.CodigoPostal?.Localidade
            ?? entidade?.Rua?.CodigoPostal?.Localidade;

        return TruncateOptional(localidade, LocalidadeClienteMax);
    }

    public static string? ResolverNumeroContribuinteCliente(
        string? requestNif,
        Entidade? entidadePrimaria,
        Entidade? entidadeSecundaria = null)
    {
        string? nif = requestNif
            ?? entidadePrimaria?.NumeroContribuinte
            ?? entidadeSecundaria?.NumeroContribuinte;

        return TruncateOptional(nif, NumeroContribuinteClienteMax);
    }
}
