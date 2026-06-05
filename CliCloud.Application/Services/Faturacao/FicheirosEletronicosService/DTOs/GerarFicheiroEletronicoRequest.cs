namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

public class GerarFicheiroEletronicoRequest
{
    public Guid DocumentoId { get; set; }
    public string Sigla { get; set; } = string.Empty;
}