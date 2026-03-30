using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.CartasConducao.CartaConducaoRestricoesService.DTOs
{
  public class CartaConducaoRestricoesLightDTO : IDto
  {
    public Guid Id { get; set; }
    public int CodigoRestricao { get; set; }
    public string? Descricao { get; set; }
  }
}
