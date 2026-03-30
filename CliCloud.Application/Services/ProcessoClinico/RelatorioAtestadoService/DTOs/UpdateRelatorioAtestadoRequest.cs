using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.RelatorioAtestadoService.DTOs
{
    public class UpdateRelatorioAtestadoRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public Guid MedicoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string TextoHtml { get; set; } = null!;
    }

    public class UpdateRelatorioAtestadoValidator : AbstractValidator<UpdateRelatorioAtestadoRequest>
    {
        public UpdateRelatorioAtestadoValidator()
        {
            _ = RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
            _ = RuleFor(x => x.TextoHtml).NotEmpty();
        }
    }
}

