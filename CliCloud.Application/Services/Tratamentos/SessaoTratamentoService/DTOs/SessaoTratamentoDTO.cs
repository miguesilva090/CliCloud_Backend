using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.SessaoTratamentoService.DTOs
{
  public class SessaoTratamentoDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public Guid TratamentoId { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }

    public Guid? FisioterapeutaId { get; set; }
    public Guid? AuxiliarId { get; set; }
    public Guid? OutroTecnicoId { get; set; }

    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }
    public string? DuracaoFisio { get; set; }
    public string? DuracaoAux { get; set; }
    public string? DuracaoOutro { get; set; }

    public Guid? ReciboId { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }
    public string? NumTransacao { get; set; }

    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    public int? Faltou { get; set; }
    public int? CompensaFalta { get; set; }
    public string? ObsFalta { get; set; }
    public int? Desmarcado { get; set; }

    public string? Destino { get; set; }
    public int? ConfFact { get; set; }
    public string? Obs { get; set; }
    public string? ObservSessao { get; set; }
    public int? HistSess { get; set; }
    public int? TipoCambio { get; set; }
    public Guid? TipoDocumentoId { get; set; }
    public Guid? DocumentoId { get; set; }
    public DateTime? DataApagar { get; set; }
  }
}

