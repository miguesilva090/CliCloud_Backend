using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.GrauAlergiaService.DTOs
{
    public class CreateGrauAlergiaRequest : IDto
    {
        public string Descricao { get; set; } = string.Empty;
    }

    public class CreateGrauAlergiaValidator : AbstractValidator<CreateGrauAlergiaRequest>
    {
        public CreateGrauAlergiaValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}
