namespace CliCloud.Application.Services.Faturacao.CredenciaisSnsService;

public static class CredenciaisSnsModulo 
{
    public const string Especialidades = "especialidades";
    public const string Fisioterapia = "fisioterapia";
    public const string Exames = "exames";


    public static bool TryParse(string? value, out string modulo)
    {
        modulo = (value ?? "").Trim().ToLowerInvariant();
        return modulo is Especialidades or Fisioterapia or Exames;
    }

}