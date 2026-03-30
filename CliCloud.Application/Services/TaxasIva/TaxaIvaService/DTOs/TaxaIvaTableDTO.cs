using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs
{
    public class TaxaIvaTableDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Taxa { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
