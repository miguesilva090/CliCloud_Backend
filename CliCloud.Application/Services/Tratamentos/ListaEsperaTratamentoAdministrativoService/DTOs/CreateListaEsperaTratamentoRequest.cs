using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.ListaEsperaTratamentoAdministrativoService.DTOs;

public class CreateListaEsperaTratamentoRequest : IDto
{
    public Guid UtenteId { get; set; }
    public Guid? MedicoId { get; set; }
    public Guid? OrganismoId { get; set; }
    public Guid? PrioridadeId { get; set; }
    public Guid? EstadoListaEsperaId { get; set; }
    public Guid? LocalTratamentoId { get; set; }
    public Guid? PatologiaId { get; set; }
    public Guid? SinistradoId { get; set; }
    public Guid? SeguradoraId { get; set; }
    public string? Designacao { get; set; }
    public int? NumSessoes { get; set; }
    public string? HoraDesejada { get; set; }
    public int? NFaltMax { get; set; }
    public int? NFaltComax { get; set; }
    public string? Credencial { get; set; }
    public DateTime? ValidadeCredencial { get; set; }
    public int? TaxaModeradora { get; set; }
    public string? Obs { get; set; }
    public string? TecObs { get; set; }
    public string? DuracaoTotal { get; set; }
    public bool CredencialExterna { get; set; }
    public int? Ordem { get; set; }
    public IReadOnlyList<ListaEsperaTratamentoServicoRequest>? Servicos { get; set; }
}

public class CreateListaEsperaTratamentoValidator : AbstractValidator<CreateListaEsperaTratamentoRequest>
{
    public CreateListaEsperaTratamentoValidator()
    {
        RuleFor(x => x.UtenteId).NotEmpty();
        RuleFor(x => x.Designacao).MaximumLength(200);
        RuleFor(x => x.Credencial).MaximumLength(100);
        RuleFor(x => x.Obs).MaximumLength(4000);
        RuleFor(x => x.TecObs).MaximumLength(4000);
        RuleFor(x => x.HoraDesejada).MaximumLength(50);

        RuleFor(x => x.ValidadeCredencial)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.Credencial))
            .WithMessage("Indique a validade da credencial");
    }
}