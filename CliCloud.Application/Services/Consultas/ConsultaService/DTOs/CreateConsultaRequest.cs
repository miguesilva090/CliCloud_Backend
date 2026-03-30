using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ConsultaService.DTOs
{
    public class CreateConsultaRequest : IDto
    {
        public Guid? UtenteId { get; set; }
        public Guid? MedicoId { get; set; }
        public Guid? EspecialidadeId { get; set; }
        public Guid? TecnicoId { get; set; }

        public DateTime? Data { get; set; }
        public string? HoraInic { get; set; }
        public string? HoraFim { get; set; }
        public string? HoraChegada { get; set; }
        public string? HoraGdh { get; set; }
        public string? Sala { get; set; }

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
        public string? MotivoConsulta { get; set; }
        public string? Diagnostico { get; set; }
        public string? Obs { get; set; }
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

    public class CreateConsultaValidator : AbstractValidator<CreateConsultaRequest>
    {
        public CreateConsultaValidator()
        {
            _ = RuleFor(x => x.HoraInic).MaximumLength(20);
            _ = RuleFor(x => x.HoraFim).MaximumLength(20);
            _ = RuleFor(x => x.HoraChegada).MaximumLength(20);
            _ = RuleFor(x => x.HoraGdh).MaximumLength(20);
            _ = RuleFor(x => x.Sala).MaximumLength(50);
            _ = RuleFor(x => x.Credencial).MaximumLength(100);
            _ = RuleFor(x => x.Apolice).MaximumLength(50);
            _ = RuleFor(x => x.NumBenif).MaximumLength(50);
            _ = RuleFor(x => x.NumDevolucao).MaximumLength(50);
            _ = RuleFor(x => x.NumDestacavel).MaximumLength(50);
            _ = RuleFor(x => x.MotivoConsulta).MaximumLength(500);
            _ = RuleFor(x => x.Diagnostico).MaximumLength(500);
            _ = RuleFor(x => x.Obs).MaximumLength(2000);
            _ = RuleFor(x => x.Destino).MaximumLength(200);
            _ = RuleFor(x => x.Utilizador).MaximumLength(100);
            _ = RuleFor(x => x.TipoConsulta).MaximumLength(100);
            _ = RuleFor(x => x.CExtramed).MaximumLength(100);
            _ = RuleFor(x => x.Senha).MaximumLength(50);
            _ = RuleFor(x => x.NumeroTFatura).MaximumLength(50);
            _ = RuleFor(x => x.NumeroTFaturaOrganismo).MaximumLength(50);
            _ = RuleFor(x => x.DescMotivoConsulta).MaximumLength(500);
            _ = RuleFor(x => x.ICD91).MaximumLength(50);
            _ = RuleFor(x => x.ICD92).MaximumLength(50);
            _ = RuleFor(x => x.IdentificadorQueue).MaximumLength(100);
            _ = RuleFor(x => x.DescricaoJust).MaximumLength(500);
        }
    }
}
