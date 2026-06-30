using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Credenciais.LoteDirectService.DTOs
{
    public class CreateLoteDirectRequest : IDto
    {
        public Guid? UtenteId { get; set; }
        public string? Credencial { get; set; }
        public int? CodigoOrganismo { get; set; }
        public int? Mes { get; set; }
        public int? Ano { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int? TipoServico { get; set; }
        public Guid? TipoServicoRegistoId { get; set; }
        public int? TipoLote { get; set; }
        public int? NumeroLote { get; set; }
        public int? IndiceLote { get; set; }
        public decimal? ValorTaxas { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorTotalV2 { get; set; }
        public decimal? ValorTotalV3 { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? ValorTaxasLinhas { get; set; }
        public bool Historico { get; set; }

        public string? CentroSaude { get; set; }
        public int? Isencao { get; set; }
        public string? Proveniencia { get; set; }
        public bool CredencialExterna { get; set; }
        public string? Servicos { get; set; }

        public Guid? MedicoId { get; set; }
        public string? CodigoMedico { get; set; }
        public string? Especialidade { get; set; }
        public Guid? MedicoExternoId { get; set; }

        public Guid? ServicoConsultaId { get; set; }

        public string? CodigoServicoConsulta { get; set; }
        public string? ServicoConsulta { get; set; }
        public string? CodigoSubsistemaConsulta { get; set; }
        public int? QuantidadeConsulta { get; set; }
        public decimal? ValorConsulta { get; set; }
        public decimal? TaxaConsulta { get; set; }
        public bool ProcedimentosEfetuados { get; set; }

        public List<LoteDirectLinhaUpsertRequest>? Linhas { get; set; }
        public List<LoteDirectLinhaUpsertRequest>? Linhas789 { get; set; }
    }

    public class CreateLoteDirectValidator : AbstractValidator<CreateLoteDirectRequest>
    {
        public CreateLoteDirectValidator()
        {
            _ = RuleFor(x => x.UtenteId)
                .NotEmpty()
                .WithMessage("Utente em falta.");

            _ = RuleFor(x => x.Credencial)
                .NotEmpty()
                .WithMessage("Nº credencial em falta.");

            _ = RuleFor(x => x.Mes)
                .InclusiveBetween(1, 12)
                .WithMessage("Mês inválido.");

            _ = RuleFor(x => x.Ano)
                .GreaterThanOrEqualTo(1900)
                .WithMessage("Ano inválido.");
        }
    }
}
