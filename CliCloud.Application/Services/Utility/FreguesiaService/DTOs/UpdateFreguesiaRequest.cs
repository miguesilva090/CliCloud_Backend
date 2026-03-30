using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;


namespace CliCloud.Application.Services.Utility.FreguesiaService.DTOs
{
    public class UpdateFreguesiaRequest : IDto
    {
        public required string Nome { get; set; }
        public required string ConcelhoId { get; set; }
    }

    public class UpdateFreguesiaValidator : AbstractValidator<UpdateFreguesiaRequest>
    {
        public UpdateFreguesiaValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.ConcelhoId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("ConcelhoId deve ser um GUID válido e não estar vazio.");
        }
    }
}

