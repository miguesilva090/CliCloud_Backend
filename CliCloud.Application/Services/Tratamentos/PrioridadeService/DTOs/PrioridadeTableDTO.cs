using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PrioridadeService.DTOs
{
    public class PrioridadeTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
