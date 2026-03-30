using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.ExameService.DTOs
{
  public class UpdateExameLinhaRequest
  {
    public Guid? Id { get; set; }
    public Guid TipoExameId { get; set; }
    public int Quantidade { get; set; } = 1;
    public string? Recomendacoes { get; set; }
  }

  public class UpdateExameRequest : IDto
  {
    public Guid UtenteId { get; set; }
    public Guid MedicoId { get; set; }
    public DateTime DataPrescricao { get; set; }
    public Guid? PrioridadeId { get; set; }
    public string? NumeroPrescricao { get; set; }
    public Guid? OrganismoId { get; set; }
    public string? Observacoes { get; set; }
    public List<UpdateExameLinhaRequest> Linhas { get; set; } = new();
  }

  public class UpdateExameValidator : AbstractValidator<UpdateExameRequest>
  {
    public UpdateExameValidator()
    {
      _ = RuleFor(x => x.UtenteId).NotEmpty();
      _ = RuleFor(x => x.MedicoId).NotEmpty();
      _ = RuleFor(x => x.DataPrescricao).NotEmpty();
      _ = RuleFor(x => x.NumeroPrescricao).MaximumLength(50);
    }
  }
}
