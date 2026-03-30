using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Doencas.DoencaService.DTOs
{
    public class UpdateDoencaRequest : IDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Code { get; set; }
    }

    public class UpdateDoencaValidator : AbstractValidator<UpdateDoencaRequest>
    {
        public UpdateDoencaValidator()
        {
            _ = RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            _ = RuleFor(x => x.Code).MaximumLength(20);
        }
    }
}
