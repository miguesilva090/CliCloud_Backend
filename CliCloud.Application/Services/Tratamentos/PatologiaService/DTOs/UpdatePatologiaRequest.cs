using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Tratamentos.PatologiaService.DTOs
{
  public class UpdatePatologiaRequest : IDto
  {
    public string Designacao { get; set; } = string.Empty;
    public Guid? LocalTratamentoId { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? EspecificacaoTecnica { get; set; }
    public string? Doencas { get; set; }
    public bool Inativo { get; set; }
    public IEnumerable<CreatePatologiaServicoRequest>? PatologiaServicos { get; set; }
    public IEnumerable<Guid>? DoencaIds { get; set; }
  }

  public class UpdatePatologiaValidator : AbstractValidator<UpdatePatologiaRequest>
  {
    public UpdatePatologiaValidator()
    {
      _ = RuleFor(x => x.Designacao)
        .NotEmpty()
        .MaximumLength(100)
        .WithMessage("Designação é obrigatória e deve ter no máximo 100 caracteres.");
    }
  }
}
