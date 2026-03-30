using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class TipoExameLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
    }
}
