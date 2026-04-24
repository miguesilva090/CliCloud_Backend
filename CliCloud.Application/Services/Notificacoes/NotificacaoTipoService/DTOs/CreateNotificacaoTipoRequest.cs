using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Notificacoes.NotificacaoTipoService.DTOs;

public class CreateNotificacaoTipoRequest : IDto
{
  public required string DesignacaoTipo { get; set; }
  public bool ReservadoSistema { get; set; }
}

public class CreateNotificacaoTipoValidator : AbstractValidator<CreateNotificacaoTipoRequest>
{
  public CreateNotificacaoTipoValidator()
  {
    _ = RuleFor(x => x.DesignacaoTipo)
      .NotEmpty()
      .MaximumLength(60)
      .WithMessage("Designação é obrigatória (máx. 60 caracteres).");
  }
}
