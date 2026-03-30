using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.AnalisesService.DTOs
{
    public class UpdateAnaliseRequest : IDto
    {
        public required string Nome { get; set; }
        public string? UnidadeMedida { get; set; }
        public string? ValoresReferencia { get; set; }
    }

    public class UpdateAnaliseValidator : AbstractValidator<UpdateAnaliseRequest>
    {
        public UpdateAnaliseValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.UnidadeMedida).MaximumLength(50);
            _ = RuleFor(x => x.ValoresReferencia).MaximumLength(200);
        }
    }
}
