using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utentes.UtentePatologiaComparticipacaoService.DTOs
{
    public class UtentePatologiaComparticipacaoDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public int CodigoComparticipacao { get; set; }
        public string? Designacao { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}