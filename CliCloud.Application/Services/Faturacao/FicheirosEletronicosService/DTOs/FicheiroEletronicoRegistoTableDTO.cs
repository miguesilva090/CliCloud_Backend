namespace CliCloud.Application.Services.Faturacao.FicheirosEletronicosService.DTOs;

public class FIcheiroEletronicoRegistoTableDTO
{
    public Guid Id { get; set; }
    public Guid DocumentoId { get; set; }
    public string? NumeroExibicaoDocumento { get; set; }
    public string Sigla { get; set; } = string.Empty;
    public DateTime DataGeracao { get; set; }
    public DateTime? DataDocumento { get; set; }
}