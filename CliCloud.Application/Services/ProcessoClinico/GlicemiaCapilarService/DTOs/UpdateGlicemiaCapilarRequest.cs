using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.GlicemiaCapilarService.DTOs
{
    public class UpdateGlicemiaCapilarRequest : IDto
    {
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateGlicemiaCapilarValidator : AbstractValidator<UpdateGlicemiaCapilarRequest>
    {
        public UpdateGlicemiaCapilarValidator()
        {
            _ = RuleFor(x => x.Name).NotEmpty();
        }
    }
}

