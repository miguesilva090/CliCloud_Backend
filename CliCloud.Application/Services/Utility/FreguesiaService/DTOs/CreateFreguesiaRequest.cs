using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Utility.FreguesiaService.DTOs
{
    public class CreateFreguesiaRequest : IDto
    {
        public required string Nome { get; set; }
        public required string ConcelhoId { get; set; }
    }

    public class CreateFreguesiaValidator : AbstractValidator<CreateFreguesiaRequest>
    {
        public CreateFreguesiaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
        }
    }
}
