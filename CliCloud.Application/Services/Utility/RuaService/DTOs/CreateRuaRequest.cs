using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Utility.RuaService.DTOs
{
    public class CreateRuaRequest : IDto
    {
        public required string Nome { get; set; }
        public required string FreguesiaId { get; set; }
        public required string CodigoPostalId { get; set; }
    }

    public class CreateRuaValidator : AbstractValidator<CreateRuaRequest>
    {
        public CreateRuaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
        }
    }
}
