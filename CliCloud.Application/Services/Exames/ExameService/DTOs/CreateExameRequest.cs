using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  public class CreateExameLinhaRequest
  {
    public Guid TipoExameId { get; set; }
    public int Quantidade { get; set; } = 1;
    public string? Recomendacoes { get; set; }
  }

  public class CreateExameRequest : IDto
  {
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public DateTime DataPrescricao { get; set; }
    public Guid? PrioridadeId { get; set; }
    public string? NumeroPrescricao { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? Observacoes { get; set; }
    public List<CreateExameLinhaRequest> Linhas { get; set; } = new();
  }

  public class CreateExameValidator : AbstractValidator<CreateExameRequest>
  {
    public CreateExameValidator()
    {
      _ = RuleFor(x => x.UtenteId).NotEmpty();
      _ = RuleFor(x => x.MedicoId).NotEmpty();
      _ = RuleFor(x => x.DataPrescricao).NotEmpty();
      _ = RuleFor(x => x.NumeroPrescricao).MaximumLength(50);
    }
  }
}
