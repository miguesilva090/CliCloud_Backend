using FluentValidation;

namespace CliCloud.Application.Services.Utility.ReplicarSubsistemasService.DTOs;

public class ReplicarSubsistemasRequest
{
    public Guid OrganismoOrigemId { get; set; }
    public Guid OrganismoDestinoId { get; set; }
}

public class ReplicarSubsistemasResponse
{
    public int TotalOrigem { get; set; }
    public int RemovidosDestino { get; set; }
    public int CriadosDestino { get; set; }
}

public class ReplicarSubsistemasValidator : AbstractValidator<ReplicarSubsistemasRequest>
{
    public ReplicarSubsistemasValidator()
    {
        RuleFor(x => x.OrganismoOrigemId).NotEmpty();
        RuleFor(x => x.OrganismoDestinoId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.OrganismoOrigemId != x.OrganismoDestinoId)
            .WithMessage("Organismo de origem e destino não podem ser iguais");
    }
}
