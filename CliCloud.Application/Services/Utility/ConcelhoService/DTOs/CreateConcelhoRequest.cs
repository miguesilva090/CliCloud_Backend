using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Utility.ConcelhoService.DTOs
{
    public class CreateConcelhoRequest : IDto
    {
        public required string Nome { get; set; }
        public required string DistritoId { get; set; }
    }

    public class CreateConcelhoValidator : AbstractValidator<CreateConcelhoRequest>
    {
        public CreateConcelhoValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.DistritoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("DistritoId deve ser um GUID válido e não estar vazio.");
        }
    }
}
