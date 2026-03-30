using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.TemperaturaCorporalService.DTOs
{
    public class UpdateTemperaturaCorporalRequest : IDto
    {
        public string Name { get; set; }
    }

    public class UpdateTemperaturaCorporalValidator : AbstractValidator<UpdateTemperaturaCorporalRequest>
    {
        public UpdateTemperaturaCorporalValidator()
        {
            _ = RuleFor(x => x.Name).NotEmpty();
        }
    }
}

