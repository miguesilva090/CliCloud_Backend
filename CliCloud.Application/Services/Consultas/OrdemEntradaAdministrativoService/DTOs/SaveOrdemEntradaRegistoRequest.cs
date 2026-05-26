using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.OrdemEntradaAdministrativoService.DTOs;

public class SaveOrdemEntradaRegistoRequest : IDto 
{
    public Guid UtenteId { get; set; }
    public Guid OrganismoId { get; set; }
    public Guid MedicoId { get; set; }
    public Guid TipoAdmissaoId { get; set; }
    public Guid TipoConsultaId { get; set; }
    public Guid? SalaId { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan? Duracao { get; set; }
    public string? Observacoes { get; set; }

}

public class SaveOrdemEntradaRegistoValidator : AbstractValidator<SaveOrdemEntradaRegistoRequest>
{
    public SaveOrdemEntradaRegistoValidator()
    {
        RuleFor(x => x.UtenteId).NotEmpty();
        RuleFor(x => x.OrganismoId).NotEmpty();
        RuleFor(x => x.MedicoId).NotEmpty();
        RuleFor(x => x.TipoAdmissaoId).NotEmpty();
        RuleFor(x => x.TipoConsultaId).NotEmpty();
        RuleFor(x => x.Data).NotEmpty();
        RuleFor(x => x.HoraInicio).NotEmpty();
    }
}