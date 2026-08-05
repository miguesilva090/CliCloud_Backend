#nullable enable

using System.ComponentModel.DataAnnotations.Schema;
using CliCloud.Domain.Entities.Common;
using CliCloud.Domain.Entities.Documentos;
using CliCloud.Domain.Entities.Medicos;
using CliCloud.Domain.Entities.Seguradoras;
using CliCloud.Domain.Entities.Organismos;
using CliCloud.Domain.Entities.Tecnicos;
using CliCloud.Domain.Entities.Utentes;

namespace CliCloud.Domain.Entities.Tratamentos
{
  [Table("Tratamento", Schema = "Tratamentos")]
  public class Tratamento : AuditableEntityWithSoftDelete
  {
    // Relacionamentos principais
    public Guid? UtenteId { get; set; }
    public Utente? Utente { get; set; }
    
    public Guid? MedicoId { get; set; }
    public Medico? Medico { get; set; }
    
    public Guid? FisioterapeutaId { get; set; }
    public Tecnico? Fisioterapeuta { get; set; }
    
    public Guid? AuxiliarId { get; set; }
    public Tecnico? Auxiliar { get; set; }
    
    public Guid? OutroTecnicoId { get; set; }
    public Tecnico? OutroTecnico { get; set; }
    
    public Guid? OrganismoId { get; set; }
    public Organismo? Organismo { get; set; }
    
    public Guid? LocalTratamentoId { get; set; }
    public LocalTratamento? LocalTratamento { get; set; }
    public Guid? TratamentoPredId { get; set; }
    public Tratamento? TratamentoPred { get; set; }
    public Guid? LocalOrigemId { get; set; }
    
    // Dados do tratamento
    public string? Designacao { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? DataInic { get; set; }
    public int? ConfDfim { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? Data { get; set; }
    
    // Faltas
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public int? NFalta { get; set; }
    public int? NFaltaCons { get; set; }
    public int? NAltSess { get; set; }
    
    // Financeiro
    public double? Preco { get; set; }
    public double? DescInst { get; set; }
    public double? DescCli { get; set; }
    public double? ValorDesc { get; set; }
    public Guid? ReciboId { get; set; }
    public Recibo? Recibo { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }
    
    // Status
    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    public int? Suspenso { get; set; }
    public DateTime? DataSuspensao { get; set; }
    public int? Provisorio { get; set; }
    
    // Outros campos
    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public int? Isencao { get; set; }
    public string? Credencial { get; set; }
    public int? CredencialExterna { get; set; }
    public int? DestacavelCredencial { get; set; }
    public int? TaxaMod { get; set; }
    public int? Inisess { get; set; }
    public string? HoraFisio { get; set; }
    public string? HoraAux { get; set; }
    public string? HoraOutro { get; set; }
    public string? DuracaoTotal { get; set; }
    public int? UnidadeTempoFisio { get; set; }
    public int? UnidadeTempoAux { get; set; }
    public int? UnidadeTempoOutro { get; set; }
    public int? SelOutro { get; set; }
    public string? NumCartao { get; set; }
    public bool? Orespons { get; set; }
    public int? ConfirmaLoc { get; set; }
    public Guid? SinistroId { get; set; }
    public Guid? SeguradoraId { get; set; }
    public Seguradora? Seguradora { get; set; }
    public Guid? DocumentoId { get; set; }
    public Documento? Documento { get; set; }
    public int? SemanaCompleta { get; set; }
    public int? VemListEsp { get; set; }
    public Guid? ListaEsperaTratamentoId { get; set; }
    public ListaEsperaTratamento? ListaEsperaTratamento { get; set; }
    public int? CartaoDevolv { get; set; }
    public int TerapiaFala { get; set; }
    public string? NumBenif { get; set; }
    public string? Apolice { get; set; }
    public string? NomePatologia { get; set; }
    public bool? Frespons { get; set; }
    public bool? Arespons { get; set; }
    public int Lotes { get; set; }
    
    // Relacionamentos filhos
    public ICollection<SessaoTratamento> Sessoes { get; set; } = [];
    public ICollection<ServicoTratamento> Servicos { get; set; } = [];
  }
}
