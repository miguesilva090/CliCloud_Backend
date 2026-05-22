using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.ListaEsperaAdministrativoService.DTOs;

public class CreateListaEsperaRequest : IDto 
{
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid EspecialidadeId { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? PrioridadeId { get; set; }
    public Guid? TipoConsultaId { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan? HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public string? Credencial { get; set; }
    public string? Obs { get; set; }
}

public class CreateListaEsperaValidator : AbstractValidator<CreateListaEsperaRequest>
{
    public CreateListaEsperaValidator()
    {
        _ = RuleFor(x => x.UtenteId).NotEmpty();
        _ = RuleFor(x => x.EspecialidadeId).NotEmpty();
        _ = RuleFor(x => x.Data).NotEmpty();
        _ = RuleFor(x => x.Credencial).MaximumLength(100);
        _ = RuleFor(x => x.Obs).MaximumLength(4000);
        _ = RuleFor(x => x.HoraFim)
            .Must((dto, fim) => !fim.HasValue || !dto.HoraInicio.HasValue || fim > dto.HoraInicio)
            .WithMessage("A Hora de Fim deve ser superior à Hora de Início");
    }
}