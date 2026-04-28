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
  /// <summary>Um destinatário (compatível com API anterior).</summary>
  public Guid? DestinatarioUtilizadorId { get; set; }
  /// <summary>Vários destinatários — mesmo comportamento que <c>tipoDestinatario == 1</c> no WS legado.</summary>
  public List<Guid>? DestinatariosUtilizadorIds { get; set; }
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
    _ = RuleFor(x => x.Descricao)
      .Must(s => !string.IsNullOrWhiteSpace(s))
      .WithMessage("Descrição é obrigatória.");
    _ = RuleFor(x => x.DestinatariosUtilizadorIds)
      .Must(list => list == null || list.TrueForAll(id => id != Guid.Empty))
      .WithMessage("Lista de destinatários contém identificadores inválidos.");
  }
}
