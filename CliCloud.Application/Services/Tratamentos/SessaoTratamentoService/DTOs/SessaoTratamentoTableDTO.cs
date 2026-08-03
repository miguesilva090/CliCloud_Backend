using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs
{
  public class SessaoTratamentoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public Guid TratamentoId { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? Duracao { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public int? Faltou { get; set; }
    public int? Confirmado { get; set; }
    public int? Efetuado { get; set; }
    public int? Desmarcado { get; set; }
    public DateTime CreatedOn { get; set; }
    public int ServicosCount { get; set; }
    public int? CompensaFalta { get; set; }
  }
}

