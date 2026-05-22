using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class CreateMarcacaoAdministrativoRequest : IDto
{
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? EspecialidadeId { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public Guid? TipoAdmissaoId { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
}

public class CreateMarcacaoAdministrativoRequestValidator : AbstractValidator<CreateMarcacaoAdministrativoRequest>
{
    public CreateMarcacaoAdministrativoRequestValidator()
    {
        _ = RuleFor(x => x.UtenteId).NotEmpty();
        _ = RuleFor(x => x.OrganismoId).NotEmpty().WithMessage("Organismo é obrigatório.");
        _ = RuleFor(x => x.Data).NotEmpty();
        _ = RuleFor(x => x.HoraInicio).NotEmpty();
        _ = RuleFor(x => x.Credencial).MaximumLength(100);
        _ = RuleFor(x => x.Obs).MaximumLength(2000);

        _ = RuleFor(x => x.HoraFim)
            .Must((dto, horaFim) => !horaFim.HasValue || horaFim > dto.HoraInicio)
            .WithMessage("HoraFim deve ser superior à HoraInicio.");
    }
}
