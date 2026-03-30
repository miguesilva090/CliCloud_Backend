using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;
using CliCloud.Domain.Enums;

namespace CliCloud.Application.Services.EntidadesFinanceiras.EntidadeFinanceiraService.DTOs
{
    public class UpdateEntidadeFinanceiraRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        public string? Email { get; set; }
        public string? NumeroContribuinte { get; set; }
        public string? RuaId { get; set; }
        public string? CodigoPostalId { get; set; }
        public string? FreguesiaId { get; set; }
        public string? ConcelhoId { get; set; }
        public string? DistritoId { get; set; }
        public string? PaisId { get; set; }
        public string? NumeroPorta { get; set; }
        public string? AndarRua { get; set; }
        public string? Observacoes { get; set; }
        public int? Status { get; set; }
        public string? UrlFoto { get; set; }
        public IEnumerable<UpsertEntidadeContactoItemRequest>? EntidadeContactos { get; set; }

        // Campos específicos de EntidadeFinanceira
        public string? Abreviatura { get; set; }
        public required string PaisPrefixo { get; set; }
        public required string TipoEntidadeFinanceiraId { get; set; }
        public CondicaoSns? CondicaoSns { get; set; }
    }

    public class UpdateEntidadeFinanceiraValidator : AbstractValidator<UpdateEntidadeFinanceiraRequest>
    {
        public UpdateEntidadeFinanceiraValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.TipoEntidadeId)
              .NotEmpty()
              .Equal(10) // EntidadeFinanceira = 10
              .WithMessage("TipoEntidadeId deve ser 10 (EntidadeFinanceira).");

            _ = RuleFor(x => x.Email)
              .EmailAddress()
              .When(x => !string.IsNullOrWhiteSpace(x.Email));
            _ = RuleFor(x => x.UrlFoto)
              .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
              .WithMessage("UrlFoto deve ser uma URL válida.");

            // Validações específicas de EntidadeFinanceira
            _ = RuleFor(x => x.PaisPrefixo)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("PaisPrefixo é obrigatório e deve ter no máximo 20 caracteres.");
            _ = RuleFor(x => x.TipoEntidadeFinanceiraId)
                .NotEmpty()
                .Must(GSHelpers.BeValidGuid)
                .WithMessage("TipoEntidadeFinanceiraId deve ser um GUID válido e não estar vazio.");
        }
    }
}
