using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrausParentesco.GrauParentescoService.DTOs
{
    public class GrauParentescoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
