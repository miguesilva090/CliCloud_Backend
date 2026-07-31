#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Tecnicos;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("SessaoTratamento", Schema = "Tratamentos")]
  public class SessaoTratamento : AuditableEntityWithSoftDelete
  {
    // Relacionamento com Tratamento
    public Guid TratamentoId { get; set; }
    public Tratamento Tratamento { get; set; } = null!;
    
    // Dados da sessão
    public int? NumSessao { get; set; }
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public int? IHoraIni { get; set; }
    public string? Duracao { get; set; }
    public int? IDuraca { get; set; }
    
    // Relacionamentos com técnicos
    public Guid? FisioterapeutaId { get; set; }
    public Tecnico? Fisioterapeuta { get; set; }
    
    public Guid? AuxiliarId { get; set; }
    public Tecnico? Auxiliar { get; set; }
    
    public Guid? OutroTecnicoId { get; set; }
    public Tecnico? OutroTecnico { get; set; }
    
    // Horários específicos
    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }
    public string? DuracaoFisio { get; set; }
    public string? DuracaoAux { get; set; }
    public string? DuracaoOutro { get; set; }
    
    // Financeiro
    public Guid? ReciboId { get; set; }
    public Recibo? Recibo { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }
    public string? NumTransacao { get; set; }
    
    // Status
    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    /// <summary>Presente (legado SESSTRAT.confirmado).</summary>
    public int? Confirmado { get; set; }
    /// <summary>Sessão efectuada (legado SESSTRAT.efetuado).</summary>
    public int? Efetuado { get; set; }
    public int? Faltou { get; set; }
    public int? CompensaFalta { get; set; }
    public string? ObsFalta { get; set; }
    public int? Desmarcado { get; set; }
    
    // Outros campos
    public string? Destino { get; set; }
    public int? ConfFact { get; set; }
    public string? Obs { get; set; }
    public string? ObservSessao { get; set; }
    public int? HistSess { get; set; }
    public int? TipoCambio { get; set; }
    public Guid? TipoDocumentoId { get; set; }
    public TipoDocumento? TipoDocumento { get; set; }
    public Guid? DocumentoId { get; set; }
    public Documento? Documento { get; set; }
    public DateTime? DataApagar { get; set; }
    
    // Relacionamentos filhos
    public ICollection<ServicoSessao> Servicos { get; set; } = [];
  }
}
