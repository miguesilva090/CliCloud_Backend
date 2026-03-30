using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Servicos.ServicoService.DTOs
{
  public class ServicoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public Guid TipoServicoId { get; set; }
  }
}

