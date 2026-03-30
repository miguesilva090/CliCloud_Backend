using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;
using CliCloud.Application.Services.Utility.EntidadeContactoService.DTOs;

namespace CliCloud.Application.Services.Bancos.BancoService.DTOs
{
    public class CreateBancoRequest : IDto
    {
        // Campos da entidade base Entidade
        public required string Nome { get; set; }
        public required int TipoEntidadeId { get; set; }
        // Campos específicos de Banco
        public string? Abreviatura { get; set; }
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
        public IEnumerable<CreateEntidadeContactoItemRequest>? EntidadeContactos { get; set; }
    }

    public class CreateBancoValidator : AbstractValidator<CreateBancoRequest>
    {
        public CreateBancoValidator()
        {
            // Fluxo simplificado: apenas o Nome é obrigatório.
            _ = RuleFor(x => x.Nome)
              .NotEmpty()
              .WithMessage("O campo Nome é obrigatório.");

            // Restantes campos herdados de Entidade ficam totalmente opcionais neste fluxo.
            // Se no futuro precisarmos de validar Email/UrlFoto, podemos reativar as regras abaixo.
            //_ = RuleFor(x => x.Email)
            //  .EmailAddress()
            //  .When(x => !string.IsNullOrWhiteSpace(x.Email));
            //_ = RuleFor(x => x.UrlFoto)
            //  .Must(url => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _))
            //  .WithMessage("UrlFoto deve ser uma URL válida.");
        }
    }
}
