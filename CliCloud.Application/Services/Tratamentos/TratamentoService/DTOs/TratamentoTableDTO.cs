using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs
{
  public class TratamentoTableDTO : IDto
  {
    public Guid Id { get; set; }
    public string? Designacao { get; set; }
    public Guid? UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? LocalTratamentoId { get; set; }
    public DateTime? DataInic { get; set; }
    public DateTime? DataFim { get; set; }
    public int? NumSessao { get; set; }
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public int? NFalta { get; set; }
    public int? NFaltaCons { get; set; }
    public int? NAltSess { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public int? Suspenso { get; set; }
    public DateTime CreatedOn { get; set; }
    public int SessoesCount { get; set; }
    public int ServicosCount { get; set; }
    public string? OrganismoNome { get; set; }
    public string? LocalTratamentoNome { get; set; }
    public string? MedicoNome { get; set; }
    public string? NomePatologia { get; set; }
    public int? VemListEsp { get; set; }
  }
}

