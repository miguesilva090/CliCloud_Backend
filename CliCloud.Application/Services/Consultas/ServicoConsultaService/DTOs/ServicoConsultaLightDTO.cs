using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ServicoConsultaService.DTOs
{
  public class ServicoConsultaLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid ConsultaId { get; set; }
    public Guid? ServicoId { get; set; }
    public int Linha { get; set; }
    public decimal? ValorUt { get; set; }
  }
}

