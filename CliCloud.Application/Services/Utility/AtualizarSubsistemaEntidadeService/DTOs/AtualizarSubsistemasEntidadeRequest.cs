using FluentValidation;

namespace CliCloud.Application.Services.Utility.AtualizarSubsistemasEntidadeService.DTOs;

public class AtualizarSubsistemasEntidadeRequest
{
    public Guid OrganismoOrigemId { get; set; }
    public Guid OrganismoDestinoId { get; set; }
}

public class AtualizarSubsistemasEntidadeResponse
{
    public int TotalOrigem { get; set; }
    public int JaExistiamDestino { get; set; }
    public int CriadosDestino { get; set; }
}

public class AtualizarSubsistemasEntidadeValidator : AbstractValidator<AtualizarSubsistemasEntidadeRequest>
{
    public AtualizarSubsistemasEntidadeValidator()
    {
        RuleFor(x => x.OrganismoOrigemId).NotEmpty();
        RuleFor(x => x.OrganismoDestinoId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.OrganismoOrigemId != x.OrganismoDestinoId)
            .WithMessage("Organismo de origem e destino não podem ser iguais");
    }
}