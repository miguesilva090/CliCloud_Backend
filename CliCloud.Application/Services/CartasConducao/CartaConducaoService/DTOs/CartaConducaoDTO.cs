using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoService.DTOs
{
  public class CartaConducaoDTO : IDto
  {
    public Guid Id { get; set; }
    public string? CodigoCarta { get; set; }
    public string? Descricao { get; set; }
    public int Grupo { get; set; }
    public bool Inativo { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }
  }
}
