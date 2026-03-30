using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.ReportService.DTOs
{
  public class RevertReportRequest : IDto
  {
    public required string ReportName { get; set; }
  }

  public class RevertReportValidator : AbstractValidator<RevertReportRequest>
  {
    public RevertReportValidator()
    {
      _ = RuleFor(x => x.ReportName)
        .NotEmpty()
        .WithMessage("Nome do relatório é obrigatório")
        .MaximumLength(255)
        .WithMessage("O nome do relatório não pode exceder 255 caracteres");
    }
  }
}