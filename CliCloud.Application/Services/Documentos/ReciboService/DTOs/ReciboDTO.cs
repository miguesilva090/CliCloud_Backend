using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.ReciboService.DTOs
{
  public class ReciboDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TipoDocumentoId { get; set; }
    public int NumeroDocumento { get; set; }
    public DateTime? Data { get; set; }
    public Guid? UtenteId { get; set; }
    public Guid? OrganismoId { get; set; }
    public decimal? TotalDocumento { get; set; }
    public decimal? TotalLiquido { get; set; }
    public int? Estado { get; set; }
    public bool Liquidado { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
