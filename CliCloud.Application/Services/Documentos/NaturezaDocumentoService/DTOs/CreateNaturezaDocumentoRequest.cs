using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs
{
    public class CreateNaturezaDocumentoRequest : IDto
    {
        public required string Sigla { get; set; }
        public required string Descricao { get; set; }
    }

    public class CreateNaturezaDocumentoValidator : AbstractValidator<CreateNaturezaDocumentoRequest>
    {
        public CreateNaturezaDocumentoValidator()
        {
            _ = RuleFor(x => x.Sigla)
                .NotEmpty()
                .Length(1)
                .WithMessage("Sigla deve ter exatamente 1 caracter.");
            
            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}