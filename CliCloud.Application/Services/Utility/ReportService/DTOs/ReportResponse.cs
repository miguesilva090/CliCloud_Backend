using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Utility.ReportService.DTOs
{
  public class SaveReportRequest : IDto
  {
    public required string Filename { get; set; }
    public required string Content { get; set; }
  }

  public class SaveReportValidator : AbstractValidator<SaveReportRequest>
  {
    public SaveReportValidator()
    {
      _ = RuleFor(x => x.Filename)
        .NotEmpty()
        .WithMessage("O nome do ficheiro é obrigatório")
        .MaximumLength(255)
        .WithMessage("O nome do ficheiro não pode exceder 255 caracteres");

        _ = RuleFor(x => x.Content)
          .NotEmpty()
          .WithMessage("O conteúdo do ficheiro é obrigatório")
          .Must(BeValidBase64)
          .WithMessage("O conteúdo do ficheiro deve ser um Base64 válido");
    }

    private static bool BeValidBase64(string? value)
    {
      if(string.IsNullOrWhiteSpace(value))
      {
        return false;
      }
      try
      {
        _ = Convert.FromBase64String(value);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}