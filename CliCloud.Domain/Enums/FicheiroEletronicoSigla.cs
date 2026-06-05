namespace CliCloud.Domain.Enums;

public enum FicheiroEletronicoSigla
{
    SadGnr = 1,
    Adm = 2,
    SadPsp = 3,
}

public static class FicheiroEletronicoSiglaExtensions
{
    public static string ToLegadoSigla(this FicheiroEletronicoSigla sigla) => sigla switch
    {
        FicheiroEletronicoSigla.SadGnr => "SAD/GNR",
        FicheiroEletronicoSigla.Adm => "ADM",
        FicheiroEletronicoSigla.SadPsp => "SAD/PSP",
        _ => string.Empty,
    };

    public static string ToDesignacao(this FicheiroEletronicoSigla sigla) => sigla switch
    {
        FicheiroEletronicoSigla.SadGnr => "SAD/GNR",
        FicheiroEletronicoSigla.Adm => "ADM",
        FicheiroEletronicoSigla.SadPsp => "SAD/PSP",
        _ => string.Empty,
    };

    public static bool TryParse(string? value, out FicheiroEletronicoSigla sigla)
    {
        sigla = default;
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = value.Trim().ToUpperInvariant().Replace(" ", string.Empty);
        return normalized switch
        {
            "SAD/GNR" or "SADGNR" => Assign(FicheiroEletronicoSigla.SadGnr, out sigla),
            "ADM" => Assign(FicheiroEletronicoSigla.Adm, out sigla),
            "SAD/PSP" or "SADPSP" => Assign(FicheiroEletronicoSigla.SadPsp, out sigla),
            _ => false,
        };
    }

    private static bool Assign(FicheiroEletronicoSigla value, out FicheiroEletronicoSigla sigla)
    {
        sigla = value;
        return true;
    }
}
