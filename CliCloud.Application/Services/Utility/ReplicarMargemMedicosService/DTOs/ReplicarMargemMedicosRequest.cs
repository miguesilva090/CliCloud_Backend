using FluentValidation;

namespace CliCloud.Application.Services.Utility.ReplicarMargemMedicosService.DTOs;

public class ReplicarMargemMedicosRequest
{
    public Guid MedicoOrigemId { get; set; }
    public Guid MedicoDestinoId { get; set; }
}

public class ReplicarMargemMedicosResponse
{
    public int TotalOrigem { get; set; }
    public int SubstituidasDestino { get; set; }
    public int CriadasDestino { get; set; }
}

public class ReplicarMargemMedicosValidator : AbstractValidator<ReplicarMargemMedicosRequest>
{
    public ReplicarMargemMedicosValidator()
    {
        RuleFor(x => x.MedicoOrigemId).NotEmpty();
        RuleFor(x => x.MedicoDestinoId).NotEmpty();
        RuleFor(x => x)
            .Must(x => x.MedicoOrigemId != x.MedicoDestinoId)
            .WithMessage("Médico de origem e destino não podem ser iguais");
    }
}
