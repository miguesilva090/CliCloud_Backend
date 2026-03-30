using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Utility.CodigoPostalService.DTOs
{
    public class UpdateCodigoPostalRequest : IDto
    {
        public required string Codigo { get; set; }
        public required string Localidade { get; set; }
    }

    public class UpdateCodigoPostalValidator : AbstractValidator<UpdateCodigoPostalRequest>
    {
        public UpdateCodigoPostalValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Localidade).NotEmpty();
        }
    }
}

