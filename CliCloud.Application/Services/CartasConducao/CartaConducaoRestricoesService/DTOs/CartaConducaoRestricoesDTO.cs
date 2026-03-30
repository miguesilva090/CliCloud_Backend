using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs
{
  public class CartaConducaoRestricoesDTO : IDto
  {
    public Guid Id { get; set; }
    public int CodigoRestricao { get; set; }
    public string? Descricao { get; set; }
    public bool Inativo { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
