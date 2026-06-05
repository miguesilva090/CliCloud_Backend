namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

public class GerarFicheiroEletronicoResponse
{
    public string FicheiroBase64 { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string? NumeroExibicaoDocumento { get; set; }
    public DateTime? DataDocumento { get; set; }
    public List<string> Erros { get; set; } = [];
}