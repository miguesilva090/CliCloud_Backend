using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Alergias.AlergiaService.DTOs
{
    public class CreateAlergiaRequest : IDto
    {
        public string? Descricao { get; set; }
    }

    public class CreateAlergiaValidator : AbstractValidator<CreateAlergiaRequest>
    {
        public CreateAlergiaValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}
