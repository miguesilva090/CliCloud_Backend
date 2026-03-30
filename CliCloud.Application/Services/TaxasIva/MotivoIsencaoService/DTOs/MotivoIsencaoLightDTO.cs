using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.DTOs
{
    public class MotivoIsencaoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
