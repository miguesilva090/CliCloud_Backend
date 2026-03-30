using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.IndiceMassaCorporalService.DTOs
{
    public class UpdateIndiceMassaCorporalRequest : IDto
    {
        public string Name { get; set; }
    }

    public class UpdateIndiceMassaCorporalValidator : AbstractValidator<UpdateIndiceMassaCorporalRequest>
    {
        public UpdateIndiceMassaCorporalValidator()
        {
            _ = RuleFor(x => x.Name).NotEmpty();
        }
    }
}

