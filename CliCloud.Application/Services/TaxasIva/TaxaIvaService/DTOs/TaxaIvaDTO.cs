using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs
{
    public class TaxaIvaDTO : IDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Taxa { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}
