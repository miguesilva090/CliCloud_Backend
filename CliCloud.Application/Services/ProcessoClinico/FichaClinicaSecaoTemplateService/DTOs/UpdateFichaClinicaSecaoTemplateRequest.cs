using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.FichaClinicaSecaoTemplateService.DTOs
{
    public class UpdateFichaClinicaSecaoTemplateRequest : IDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
    }

    public class UpdateFichaClinicaSecaoTemplateValidator : AbstractValidator<UpdateFichaClinicaSecaoTemplateRequest>
    {
        public UpdateFichaClinicaSecaoTemplateValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.Descricao).MaximumLength(500);
        }
    }
}

