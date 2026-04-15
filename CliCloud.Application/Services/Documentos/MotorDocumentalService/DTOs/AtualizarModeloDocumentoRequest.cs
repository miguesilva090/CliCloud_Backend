using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;

    public class AtualizarModeloDocumentoRequest: IDto 
    {
        public string Nome { get; set; } = string.Empty;
        public string ConteudoHtml { get; set; } = string.Empty; 
        public bool Ativo { get; set; } = true;

    }

    public class AtualizarModeloDocumentoValidator : AbstractValidator<AtualizarModeloDocumentoRequest> 
    {
        public AtualizarModeloDocumentoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ConteudoHtml).NotEmpty();
        }
    }
