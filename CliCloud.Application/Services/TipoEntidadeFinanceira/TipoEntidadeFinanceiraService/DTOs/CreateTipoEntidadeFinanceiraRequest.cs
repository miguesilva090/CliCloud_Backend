using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.TipoEntidadeFinanceira.TipoEntidadeFinanceiraService.DTOs
{
    public class CreateTipoEntidadeFinanceiraRequest : IDto
    {
        public required string Sigla { get; set; }
        public required string Designacao { get; set; }
        public required string Dominio { get; set; }
        public required string DescricaoDominio { get; set; }
    }

    public class CreateTipoEntidadeFinanceiraValidator : AbstractValidator<CreateTipoEntidadeFinanceiraRequest>
    {
        public CreateTipoEntidadeFinanceiraValidator()
        {
            _ = RuleFor(x => x.Sigla)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Sigla é obrigatória e deve ter no máximo 20 caracteres.");
            _ = RuleFor(x => x.Designacao)
                .NotEmpty()
                .MaximumLength(254)
                .WithMessage("Designacao é obrigatória e deve ter no máximo 254 caracteres.");
            _ = RuleFor(x => x.Dominio)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Dominio é obrigatório e deve ter no máximo 20 caracteres.");
            _ = RuleFor(x => x.DescricaoDominio)
                .NotEmpty()
                .MaximumLength(254)
                .WithMessage("DescricaoDominio é obrigatória e deve ter no máximo 254 caracteres.");
        }
    }
}
