using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs
{
    public class CreateFichaClinicaSecaoTemplateRequest : IDto
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; } = true;
    }

    public class CreateFichaClinicaSecaoTemplateValidator : AbstractValidator<CreateFichaClinicaSecaoTemplateRequest>
    {
        public CreateFichaClinicaSecaoTemplateValidator()
        {
            _ = RuleFor(x => x.Codigo).MaximumLength(100);
            _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.Descricao).MaximumLength(500);
        }
    }
}
