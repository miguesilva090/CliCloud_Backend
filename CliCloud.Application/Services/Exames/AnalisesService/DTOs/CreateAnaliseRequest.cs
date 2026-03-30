using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AnalisesService.DTOs
{
    public class CreateAnaliseRequest : IDto
    {
        public required string Nome { get; set; }
        public string? UnidadeMedida { get; set; }
        public string? ValoresReferencia { get; set; }
    }

    public class CreateAnaliseValidator : AbstractValidator<CreateAnaliseRequest>
    {
        public CreateAnaliseValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.UnidadeMedida).MaximumLength(50);
            _ = RuleFor(x => x.ValoresReferencia).MaximumLength(200);
        }
    }
}
