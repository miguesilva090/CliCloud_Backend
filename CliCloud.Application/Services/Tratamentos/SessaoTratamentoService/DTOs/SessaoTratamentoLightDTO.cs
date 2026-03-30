using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs
{
  public class SessaoTratamentoLightDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TratamentoId { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? Duracao { get; set; }
  }
}

