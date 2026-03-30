using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs
{
  public class TratamentoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Designacao { get; set; }
    public DateTime? DataInic { get; set; }
    public DateTime? DataFim { get; set; }
    public int? NumSessao { get; set; }
    public Guid? UtenteId { get; set; }
  }
}

