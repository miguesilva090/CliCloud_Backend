using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Seguradoras.SeguradoraService.DTOs
{
  public class CreateSeguradoraRequest : IDto
  {
    public required string Nome { get; set; }
    public string? Apolice { get; set; }
    public decimal? Avenca { get; set; }
    public DateOnly? DataInicioContrato { get; set; }
    public DateOnly? DataFimContrato { get; set; }
    public string? Abreviatura { get; set; }
    public string? BancoId { get; set; }
    public string? NumeroIdentificacaoBancaria { get; set; }
  }

  public class CreateSeguradoraValidator : AbstractValidator<CreateSeguradoraRequest>
  {
    public CreateSeguradoraValidator()
    {
      _ = RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
      _ = RuleFor(x => x.Apolice).MaximumLength(15);
      _ = RuleFor(x => x.Abreviatura).MaximumLength(40);
      _ = RuleFor(x => x.BancoId).Must(s => string.IsNullOrEmpty(s) || GSHelpers.BeValidGuid(s)).WithMessage("BancoId inválido.");
    }
  }
}
