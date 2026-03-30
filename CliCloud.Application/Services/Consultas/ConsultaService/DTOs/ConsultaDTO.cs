using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs
{
  public class ConsultaDTO : IDto
  {
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    // Relacionamentos principais
    public Guid? UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public Guid? TecnicoId { get; set; }

    // Dados da consulta
    public DateTime? Data { get; set; }
    public string? HoraInic { get; set; }
    public string? HoraFim { get; set; }
    public string? HoraChegada { get; set; }
    public string? HoraGdh { get; set; }
    public string? Sala { get; set; }

    // Status e confirmação
    public bool? Confirmado { get; set; }
    public bool? Efectuado { get; set; }
    public bool? Faltou { get; set; }
    public bool EmTratamento { get; set; }
    public bool ConfirmaConsulta { get; set; }

    // Financeiro
    public Guid? ReciboId { get; set; }
    public DateTime? DataRecibo { get; set; }
    public double? Desconto { get; set; }
    public int? Pago { get; set; }
    public int? Faturado { get; set; }
    public string? NumDevolucao { get; set; }
    public string? NumDestacavel { get; set; }

    // Documentos
    public Guid? DocumentoId { get; set; }
    public Guid? TipoDocumentoId { get; set; }
    public int? EstadoU { get; set; }
    public int? EstadoI { get; set; }

    // Organismo e credenciais
    public Guid? OrganismoId { get; set; }
    public string? Credencial { get; set; }
    public int? CredencialExterna { get; set; }
    public string? Apolice { get; set; }
    public string? NumBenif { get; set; }

    // Seguradora e sinistro
    public Guid? SeguradoraId { get; set; }
    public int? Sinistrado { get; set; }
    public int? Justificacao { get; set; }
    public string? DescricaoJust { get; set; }

    // Outros campos
    public int? Isencao { get; set; }
    public Guid? TratamentoId { get; set; }
    public Guid? InstituicaoEmpregadoraId { get; set; }
    public Guid? FuncionarioId { get; set; }
    public string? Obs { get; set; }
    public string? MotivoConsulta { get; set; }
    public string? Diagnostico { get; set; }
    public string? Destino { get; set; }
    public string? Utilizador { get; set; }
    public int? Ordem { get; set; }
    public int? NumLinhas { get; set; }
    public int? NaoDiscriminar { get; set; }
    public int? TipoCambio { get; set; }
    public int? Movimento { get; set; }
    public double? ProdAplic { get; set; }
    public double? Pic { get; set; }
    public int? TipoAdmiss { get; set; }
    public string? TipoConsulta { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public string? TipoConsultaDesignacao { get; set; }
    public string? CExtramed { get; set; }
    public int? AcessoKioskSemPag { get; set; }
    public string? Senha { get; set; }
    public DateTime? DataHoraMarcacao { get; set; }
    public Guid? CodigoFatura { get; set; }
    public DateTime? DataFatura { get; set; }
    public Guid? CodigoTipoDocumentoBase { get; set; }
    public string? NumeroTFatura { get; set; }
    public Guid? CodigoFaturaOrganismo { get; set; }
    public string? NumeroTFaturaOrganismo { get; set; }
    public Guid? EspecialidadeCodigo { get; set; }
    public Guid? CodMotivoConsulta { get; set; }
    public string? DescMotivoConsulta { get; set; }
    public string? ICD91 { get; set; }
    public string? ICD92 { get; set; }
    public string? IdentificadorQueue { get; set; }
  }
}

