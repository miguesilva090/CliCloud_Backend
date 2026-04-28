using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Estomatologia.HistoriaDentariaService.DTOs
{
    public class HistoriaDentariaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public DateTime DataRegisto { get; set; }
        public string HistoriaHtml { get; set; } = string.Empty;
    }
}
