using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.DTOs
{
    public class UpdateNaturezaDocumentoRequest : IDto 
    {
        public required string Sigla { get; set; }
        public required string Descricao { get; set; }

    }

    public class UpdateNaturezaDocumentoValidator : AbstractValidator<UpdateNaturezaDocumentoRequest>
    {
        public UpdateNaturezaDocumentoValidator()
        {
            _ = RuleFor(x => x.Sigla)
                .NotEmpty()
                .Length(1)
                .WithMessage("Sigla deve ter exatamente 1 caracter");

            _ = RuleFor(x => x.Descricao)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}