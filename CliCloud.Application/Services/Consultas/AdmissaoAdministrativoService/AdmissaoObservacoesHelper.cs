namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService;

public static class AdmissaoObservacoesHelper
{
    public static string FormatarObservacaoAppend(string textoNovo, string nomeAutor, string? obsAnterior)
    {
        string linha = textoNovo.Trim();
        if(string.IsNullOrWhiteSpace(linha))
        {
            return obsAnterior ?? string.Empty;
        }

        string carimbo = $"{nomeAutor.Trim()} - {DateTime.Now:dd-MM-yyyy HH:mm}";
        string blocoNovo = $"{linha}{Environment.NewLine}{carimbo}";

        if(string.IsNullOrWhiteSpace(obsAnterior))
        {
            return blocoNovo + Environment.NewLine + Environment.NewLine;
        }

        return blocoNovo + Environment.NewLine + Environment.NewLine + obsAnterior.TrimEnd();
    }
}