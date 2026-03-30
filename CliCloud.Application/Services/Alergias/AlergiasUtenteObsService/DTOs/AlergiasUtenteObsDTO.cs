using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.AlergiasUtenteObsService.DTOs
{
    public class AlergiasUtenteObsDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public string? Observacoes { get; set; }
        public string? InformacaoImportante { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
