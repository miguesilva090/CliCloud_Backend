using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Consultas.MarcacoesAdministrativoService.DTOs;

public class SalasDisponiveisRequest : IDto
{
    public DateTime Data { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan? HoraFim { get; set; }
    public Guid? ClinicaId { get; set; }
    public Guid? IgnorarMarcacaoId { get; set; }
    public Guid? IgnorarAdmissaoId { get; set; }
}

public class SalasDisponiveisRequestValidator : AbstractValidator<SalasDisponiveisRequest>
{
    public SalasDisponiveisRequestValidator()
    {
        _ = RuleFor(x => x.Data).NotEmpty();
        _ = RuleFor(x => x.HoraInicio).NotEmpty();
    }
}