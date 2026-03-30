using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs
{
    public class TaxaIvaLightDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Taxa { get; set; }
    }
}
