using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ReportService.DTOs
{
    public class CreateReportRequest : IDto
    {
        public required string Name { get; set; }
    }

    public class CreateReportValidator : AbstractValidator<CreateReportRequest>
    {
        public CreateReportValidator()
        {
            _ = RuleFor(x => x.Name).NotEmpty();
        }
    }
}
