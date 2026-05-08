using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Sinistros.EstadoSinistroService.DTOs
{
    public class UpdateEstadoSinistroRequest : IDto 
    {
        public string Designacao { get; set; } = string.Empty;
    }

    public class UpdateEstadoSinistroValidator : AbstractValidator<UpdateEstadoSinistroRequest>
    {
        public UpdateEstadoSinistroValidator()
        {
            RuleFor(x => x.Designacao).NotEmpty().MaximumLength(80);
        }
    }
}