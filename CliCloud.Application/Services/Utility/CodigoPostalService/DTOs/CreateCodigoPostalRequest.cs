using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.CodigoPostalService.DTOs
{
    public class CreateCodigoPostalRequest : IDto
    {
        public required string Codigo { get; set; }
        public required string Localidade { get; set; }
    }

    public class CreateCodigoPostalValidator : AbstractValidator<CreateCodigoPostalRequest>
    {
        public CreateCodigoPostalValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Localidade).NotEmpty();
        }
    }
}
