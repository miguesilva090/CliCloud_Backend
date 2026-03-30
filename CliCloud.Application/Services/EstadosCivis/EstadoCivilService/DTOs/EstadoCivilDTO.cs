using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.EstadosCivis.EstadoCivilService.DTOs
{
    public class EstadoCivilDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
