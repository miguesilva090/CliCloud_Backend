using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Tratamentos.EvolucaoTratamentoFicheiroService.DTOs
{
  public class CreateEvolucaoTratamentoFicheiroRequest : IDto
  {
    public Guid EvolucaoTratamentoId { get; set; }
    public string Titulo { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string StoragePath { get; set; } = null!;
    public string? ContentType { get; set; }
    public long? TamanhoBytes { get; set; }
  }

  public class CreateEvolucaoTratamentoFicheiroRequestValidator
    : AbstractValidator<CreateEvolucaoTratamentoFicheiroRequest>
  {
    public CreateEvolucaoTratamentoFicheiroRequestValidator()
    {
      _ = RuleFor(x => x.EvolucaoTratamentoId)
        .NotEmpty().WithMessage("EvolucaoTratamentoId é obrigatório.");

      _ = RuleFor(x => x.Titulo)
        .NotEmpty().WithMessage("Título é obrigatório.")
        .MaximumLength(256);

      _ = RuleFor(x => x.FileName)
        .NotEmpty().WithMessage("Nome do ficheiro é obrigatório.")
        .MaximumLength(512);

      _ = RuleFor(x => x.StoragePath)
        .NotEmpty().WithMessage("StoragePath é obrigatório.")
        .MaximumLength(1024);
    }
  }
}

