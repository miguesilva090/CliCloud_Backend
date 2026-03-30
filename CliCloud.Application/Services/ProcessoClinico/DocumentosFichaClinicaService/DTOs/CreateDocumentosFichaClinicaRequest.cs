using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.DocumentosFichaClinicaService.DTOs
{
    public class CreateDocumentosFichaClinicaRequest : IDto
    {
        public Guid UtenteId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }

    public class CreateDocumentosFichaClinicaValidator : AbstractValidator<CreateDocumentosFichaClinicaRequest>
    {
        public CreateDocumentosFichaClinicaValidator()
        {
            _ = RuleFor(x => x.UtenteId).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500);
            _ = RuleFor(x => x.Categoria).NotEmpty();
        }
    }
}
