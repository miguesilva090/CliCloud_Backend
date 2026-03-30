using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs
{
    public class CreateRelatorioAtestadoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
    }

    public class CreateRelatorioAtestadoValidator : AbstractValidator<CreateRelatorioAtestadoRequest>
    {
        public CreateRelatorioAtestadoValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.MedicoId).NotEmpty();
            _ = RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.TextoHtml).NotEmpty();
        }
    }
}
