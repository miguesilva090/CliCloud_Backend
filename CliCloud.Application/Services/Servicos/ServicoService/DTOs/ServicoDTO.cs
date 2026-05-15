using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.ServicoService.DTOs
{
  public class ServicoDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public Guid TipoServicoId { get; set; }
    public string? TipoServicoDescricao { get; set; }
    public decimal? Preco { get; set; }
    public string? Duracao { get; set; }
    public Guid? TaxaIvaId { get; set; }
    public string? EAN { get; set; }
    public Guid? TipoAparelhoId { get; set; }
    public string? TipoAparelhoDesignacao { get; set; }
    public bool TratDentario { get; set; }
    public Guid? MotivoIsencaoId { get; set; }
    public int? CodigoMotivoIsencao { get; set; }
    public bool Inativo { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}

