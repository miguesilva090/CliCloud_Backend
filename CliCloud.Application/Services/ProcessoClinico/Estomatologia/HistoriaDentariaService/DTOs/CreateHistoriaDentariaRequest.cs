namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs
{
    public class CreateHistoriaDentariaRequest
    {
        public Guid UtenteId { get; set; }
        public string HistoriaHtml { get; set; } = string.Empty;
    }
}
