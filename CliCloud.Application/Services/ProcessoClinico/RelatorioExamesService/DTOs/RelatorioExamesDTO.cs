using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioExamesService.DTOs
{
    public class RelatorioExamesDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid UtenteId { get; set; }
        public string? Texto { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

