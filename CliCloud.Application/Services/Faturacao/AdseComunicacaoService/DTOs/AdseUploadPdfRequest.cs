namespace CliCloud.Application.Services.Faturacao.AdseComunicacaoService.DTOs;

public class AdseUploadPdfRequest
{
    public Guid DocumentoId { get; set; }
    public Guid OrigemClinicaId { get; set; }
    public string NomeFicheiro { get; set; } = string.Empty;
    public string ConteudoBase64 { get; set; } = string.Empty;
    public bool RelatorioMedico { get; set; }
}
