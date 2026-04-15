using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Domain.Enums.Documentos;

namespace CliCloud.Application.Services.Documentos.MotorDocumentalService.DTOs;
    public class CriarModeloDocumentoRequest : IDto 
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public TipoModeloDocumento Tipo { get; set; } = TipoModeloDocumento.Generico;
        public string ConteudoHtml { get; set; } = string.Empty;
    }

    public class CriarModeloDocumentoValidator : AbstractValidator<CriarModeloDocumentoRequest>
    {
        public CriarModeloDocumentoValidator()
        {
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.ConteudoHtml).MaximumLength(500000);
        }
    }
