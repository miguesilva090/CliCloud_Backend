using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;


namespace CliCloud.Application.Services.Utility.DistritoService.DTOs
{
    public class UpdateDistritoRequest : IDto
    {
        public required string Nome { get; set; }
        public required string PaisId { get; set; }
    }

    public class UpdateDistritoValidator : AbstractValidator<UpdateDistritoRequest>
    {
        public UpdateDistritoValidator()
        {
            _ = RuleFor(x => x.Nome).NotEmpty();
            _ = RuleFor(x => x.PaisId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("PaisId deve ser um GUID válido e não estar vazio.");
        }
    }
}

