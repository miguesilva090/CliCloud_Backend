namespace CliCloud.Application.Services.Tratamentos.HistoricoTratamentoAdministrativoService;

public static class HistoricoTratamentoAdministrativoModos
{
    public const string Datas = "datas";
    public const string Utentes = "utentes";
    public const string Fisioterapeuta = "fisioterapeuta";
    public const string Auxiliar = "auxiliar";
    public const string Outro = "outro";
    public const string Organismo = "organismo";
    public const string Credencial = "credencial";

    public static readonly HashSet<string> Todos = new(StringComparer.OrdinalIgnoreCase)
    {
        Datas, Utentes, Fisioterapeuta, Auxiliar, Outro, Organismo, Credencial,
    };

    public static string Normalize(string? modo)
    {
        if (string.IsNullOrWhiteSpace(modo)) return Datas;
        string m = modo.Trim().ToLowerInvariant();
        if (m is "fisioter") return Fisioterapeuta;
        return Todos.Contains(m) ? m : Datas;
    }
}