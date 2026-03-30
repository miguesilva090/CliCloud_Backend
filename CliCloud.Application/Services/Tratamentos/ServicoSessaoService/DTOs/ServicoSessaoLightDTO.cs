using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ServicoSessaoService.DTOs
{
  public class ServicoSessaoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid SessaoTratamentoId { get; set; }
    public Guid? ServicoId { get; set; }
    public string? HoraInic { get; set; }
    public string? HoraFim { get; set; }
    public decimal? Preco { get; set; }
  }
}

