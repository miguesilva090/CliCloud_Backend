using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ServicoTratamentoService.DTOs
{
  public class ServicoTratamentoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TratamentoId { get; set; }
    public Guid? ServicoId { get; set; }
    public int? Ordem { get; set; }
    public decimal? Preco { get; set; }
  }
}

