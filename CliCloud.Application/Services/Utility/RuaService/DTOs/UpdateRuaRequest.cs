using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;


namespace CliCloud.Application.Services.Utility.RuaService.DTOs
{
    public class UpdateRuaRequest : IDto
    {
        public required string Nome { get; set; }
        public required string FreguesiaId { get; set; }
        public required string CodigoPostalId { get; set; }
    }

    public class UpdateRuaValidator : AbstractValidator<UpdateRuaRequest>
    {
        public UpdateRuaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.FreguesiaId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("FreguesiaId deve ser um GUID válido e não estar vazio.");
            _ = RuleFor(x => x.CodigoPostalId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("CodigoPostalId deve ser um GUID válido e não estar vazio.");
        }
    }
}

