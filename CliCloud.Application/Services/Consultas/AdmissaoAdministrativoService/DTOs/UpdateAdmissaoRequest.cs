using FluentValidation;

namespace CliCloud.Application.Services.Consultas.AdmissaoAdministrativoService.DTOs;

public class UpdateAdmissaoRequest : CreateAdmissaoRequest
{
}

public class UpdateAdmissaoValidator : AbstractValidator<UpdateAdmissaoRequest>
{
  public UpdateAdmissaoValidator()
  {
    _ = RuleFor(x => x.UtenteId).NotEmpty();
  }
}
