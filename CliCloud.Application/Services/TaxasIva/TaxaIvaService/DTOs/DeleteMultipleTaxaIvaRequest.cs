namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.DTOs
{
    public class DeleteMultipleTaxaIvaRequest
    {
        public required IEnumerable<Guid> Ids { get; set; }
    }
}
