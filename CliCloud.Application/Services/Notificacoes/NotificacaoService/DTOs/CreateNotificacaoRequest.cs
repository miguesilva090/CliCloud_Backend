using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoService.DTOs;

public class CreateNotificacaoRequest : IDto
{
  public required string Titulo { get; set; }
  public string? Descricao { get; set; }
  public int Estado { get; set; }
  public int Prioridade { get; set; }
  public Guid NotificacaoTipoId { get; set; }
  public Guid? DestinatarioUtilizadorId { get; set; }
  public Guid? ClinicaDestinoId { get; set; }
}

public class CreateNotificacaoValidator : AbstractValidator<CreateNotificacaoRequest>
{
  public CreateNotificacaoValidator()
  {
    _ = RuleFor(x => x.Titulo)
      .NotEmpty()
      .MaximumLength(500)
      .WithMessage("Título é obrigatório (máx. 500 caracteres).");
    _ = RuleFor(x => x.NotificacaoTipoId)
      .NotEmpty()
      .WithMessage("Tipo de notificação é obrigatório.");
  }
}
