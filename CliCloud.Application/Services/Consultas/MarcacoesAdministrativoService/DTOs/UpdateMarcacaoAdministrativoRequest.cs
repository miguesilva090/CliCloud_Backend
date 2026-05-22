using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class UpdateMarcacaoAdministrativoRequest : CreateMarcacaoAdministrativoRequest, IDto
{}

public class UpdateMarcacaoAdministrativoRequestValidator : AbstractValidator<UpdateMarcacaoAdministrativoRequest>
{
    public UpdateMarcacaoAdministrativoRequestValidator() 
    {
      _ = RuleFor(x => x.UtenteId).NotEmpty();
      _ = RuleFor(x => x.Data).NotEmpty();
      _ = RuleFor(x => x.HoraInicio).NotEmpty();
      _ = RuleFor(x => x.Credencial).MaximumLength(100);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);

      _ = RuleFor(x => x.HoraFim)
        .Must((dto, horaFim) => !horaFim.HasValue || horaFim > dto.HoraInicio)
        .WithMessage("HoraFim deve ser superior à HoraInicio.");
    }
}