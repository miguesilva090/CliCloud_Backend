using FluentValidation;
using CliCloud.Application.Common.Marker;
using CliCloud.Application.Utility;

namespace CliCloud.Application.Services.Consultas.MarcacaoConsultaService.DTOs
{
  public class CreateMarcacaoConsultaRequest : IDto
  {
    public string? ConsultaId { get; set; }
    public required string UtenteId { get; set; }
    public string? MedicoId { get; set; }
    public string? EspecialidadeId { get; set; }

    public DateTime Data { get; set; }
    public required string HoraInic { get; set; }
    public string? HoraFim { get; set; }
    public string? Obs { get; set; }

    public string? OrganismoId { get; set; }
    public string? MotivoConsultaId { get; set; }
    public string? TipoAdmissaoId { get; set; }
    public string? TipoConsultaId { get; set; }
  }

  public class CreateMarcacaoConsultaValidator : AbstractValidator<CreateMarcacaoConsultaRequest>
  {
    public CreateMarcacaoConsultaValidator()
    {
      _ = RuleFor(x => x.ConsultaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("ConsultaId inválido.");
      _ = RuleFor(x => x.UtenteId).NotEmpty().Must(GSHelpers.BeValidGuid).WithMessage("UtenteId inválido.");
      _ = RuleFor(x => x.MedicoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("MedicoId inválido.");
      _ = RuleFor(x => x.EspecialidadeId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("EspecialidadeId inválido.");
      _ = RuleFor(x => x.OrganismoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("OrganismoId inválido.");
      _ = RuleFor(x => x.MotivoConsultaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("MotivoConsultaId inválido.");
      _ = RuleFor(x => x.TipoAdmissaoId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("TipoAdmissaoId inválido.");
      _ = RuleFor(x => x.TipoConsultaId).Must(id => string.IsNullOrEmpty(id) || GSHelpers.BeValidGuid(id)).WithMessage("TipoConsultaId inválido.");
      _ = RuleFor(x => x.HoraInic).NotEmpty().MaximumLength(20);
      _ = RuleFor(x => x.HoraFim).MaximumLength(20);
      _ = RuleFor(x => x.Obs).MaximumLength(2000);
    }
  }
}

