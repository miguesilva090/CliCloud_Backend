using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.TratamentoService.DTOs
{
  public class TratamentoDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public Guid? UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? FisioterapeutaId { get; set; }
    public Guid? AuxiliarId { get; set; }
    public Guid? OutroTecnicoId { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? LocalTratamentoId { get; set; }
    public Guid? TratamentoPredId { get; set; }
    public Guid? LocalOrigemId { get; set; }

    public string? Designacao { get; set; }
    public int? NumSessao { get; set; }
    public DateTime? DataInic { get; set; }
    public int? ConfDfim { get; set; }
    public DateTime? DataFim { get; set; }
    public DateTime? Data { get; set; }

    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public int? NFalta { get; set; }
    public int? NFaltaCons { get; set; }
    public int? NAltSess { get; set; }

    public double? Preco { get; set; }
    public double? DescInst { get; set; }
    public double? DescCli { get; set; }
    public double? ValorDesc { get; set; }
    public Guid? ReciboId { get; set; }
    public DateTime? DataRecibo { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }

    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }
    public int? Suspenso { get; set; }
    public DateTime? DataSuspensao { get; set; }
    public int? Provisorio { get; set; }

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
    public int? SelOutro { get; set; }
    public string? NumCartao { get; set; }
    public bool? Orespons { get; set; }
    public int? ConfirmaLoc { get; set; }
    public Guid? SinistroId { get; set; }
    public Guid? SeguradoraId { get; set; }
    public Guid? DocumentoId { get; set; }
    public int? SemanaCompleta { get; set; }
    public int? VemListEsp { get; set; }
    public Guid? ListaEsperaTratamentoId { get; set; }
    public int? CartaoDevolv { get; set; }
    public int TerapiaFala { get; set; }
    public string? NumBenif { get; set; }
    public string? Apolice { get; set; }
    public string? NomePatologia { get; set; }
    public bool? Frespons { get; set; }
    public bool? Arespons { get; set; }
    public int Lotes { get; set; }
  }

  public class UpdateTratamentoAltaRequest : IDto
  {
    public Guid Id { get; set; }
    public bool Alta { get; set; }
  }
}

