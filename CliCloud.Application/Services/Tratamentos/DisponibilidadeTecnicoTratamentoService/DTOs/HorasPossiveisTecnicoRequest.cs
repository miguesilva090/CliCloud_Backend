using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.DisponibilidadeTecnicoTratamentoService.DTOs;

public class HorasPossiveisTecnicoRequest : IDto
{
    public Guid TecnicoId { get; set; }
    public DateTime Data { get; set; }
    public int UnidadeTempo { get; set; } = 1;
    public Guid? IgnorarSessaoId { get; set; }
}

public class HorasPossiveisTecnicoValidator : AbstractValidator<HorasPossiveisTecnicoRequest>
{
    public HorasPossiveisTecnicoValidator()
    {
        _ = RuleFor(x => x.TecnicoId).NotEmpty();
        _ = RuleFor(x => x.Data).NotEmpty();
        _ = RuleFor(x => x.UnidadeTempo).InclusiveBetween(1, 50);
    }
}