using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.AparelhoService.DTOs
{
  public class AparelhoDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TipoAparelhoId { get; set; }
    public string? TipoAparelhoDesignacao { get; set; }
    public Guid? ModeloAparelhoId { get; set; }
    public string? ModeloAparelhoDesignacao { get; set; }
    public string? MarcaAparelhoDesignacao { get; set; }
    public string? CodigoSerie { get; set; }
    public string? CodigoInventario { get; set; }
    public string? Local { get; set; }
    public string? Observacoes { get; set; }
    public bool Ocupado { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
