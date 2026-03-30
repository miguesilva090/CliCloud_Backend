using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.TipoServicoService.DTOs
{
  public class TipoServicoDTO : IDto
  {
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal? TaxaModeradoraSns { get; set; }
    public bool PartilhaSemRequisicao { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}

