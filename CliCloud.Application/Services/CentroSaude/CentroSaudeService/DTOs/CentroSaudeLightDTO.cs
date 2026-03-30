using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CentroSaude.CentroSaudeService.DTOs
{
    public class CentroSaudeLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NumeroContribuinte { get; set; }
        public string? CodigoLocalCS { get; set; }
    }
}
