using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.MotivoRetencaoService.DTOs
{
    public class MotivoRetencaoLightDTO : IDto
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string TipoImposto { get; set; } = string.Empty;
    }
}
