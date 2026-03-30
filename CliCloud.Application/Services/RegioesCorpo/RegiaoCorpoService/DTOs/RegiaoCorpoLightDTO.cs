using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.RegioesCorpo.RegiaoCorpoService.DTOs
{
    public class RegiaoCorpoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
