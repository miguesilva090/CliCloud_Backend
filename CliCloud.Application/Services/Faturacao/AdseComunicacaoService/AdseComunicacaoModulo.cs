using CliCloud.Domain.Entities.Faturacao;

namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService;

public static class AdseComunicacaoModulo
{
    public const string Tratamentos = "tratamentos";
    public const string Consultas = "consultas";
    public const string Exames = "exames";

    public static bool TryParse(string? rota, out string tipoPreFaturaLegado)
    {
        tipoPreFaturaLegado = AdseEstados.TipoTratamentos;
        return (rota ?? "").Trim().ToLowerInvariant() switch
        {
            Tratamentos => Set(AdseEstados.TipoTratamentos, out tipoPreFaturaLegado),
            Consultas => Set(AdseEstados.TipoConsultas, out tipoPreFaturaLegado),
            Exames => Set(AdseEstados.TipoExames, out tipoPreFaturaLegado),
            _ => false,
        };
    }

    private static bool Set(string tipo, out string tipoPreFaturaLegado)
    {
        tipoPreFaturaLegado = tipo;
        return true;
    }
}
