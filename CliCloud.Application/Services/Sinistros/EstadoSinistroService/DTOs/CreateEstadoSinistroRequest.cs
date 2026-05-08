using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs
{
    public class CreateEstadoSinistroRequest : IDto 
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class CreateEstadoSinistroValidator : AbstractValidator<CreateEstadoSinistroRequest>
    {
        public CreateEstadoSinistroValidator()
        {
            RuleFor(x => x.Designacao).NotEmpty().MaximumLength(80);
        }
    }
}