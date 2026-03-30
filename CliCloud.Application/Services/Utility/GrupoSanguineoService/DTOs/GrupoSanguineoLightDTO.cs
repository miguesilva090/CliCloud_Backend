using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.GrupoSanguineoService.DTOs
{
    public class GrupoSanguineoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
