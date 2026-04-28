using FluentValidation;

namespace CliCloud.Application.Services.Utility.ReplicarPatologiasService.DTOs;

public class ReplicarPatologiasRequest {
    public Guid OrganismoOrigemId { get; set; }
    public Guid OrganismoDestinoId { get; set; }

    public bool SubstituirExistentes { get; set; }
}

public class ReplicarPatologiasItemResult {
    public Guid PatologiaOrigemId { get; set; }
    public Guid? PatologiaDestinoId { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? Mensagem { get; set; }
}

public class ReplicarPatologiasResponse {
    public int Total { get; set; }
    public int Criadas { get; set; }
    public int Ignoradas { get; set; }
    public List<ReplicarPatologiasItemResult> Itens { get; set; } = [];
}

public class ReplicarPatologiasValidator : AbstractValidator<ReplicarPatologiasRequest>
{
    public ReplicarPatologiasValidator()
    {
        RuleFor(x => x.OrganismoOrigemId).NotEmpty();
        RuleFor(x => x.OrganismoDestinoId).NotEmpty();
        RuleFor(x => x).Must(x => x.OrganismoOrigemId != x.OrganismoDestinoId)
            .WithMessage("Organismo de origem e destino não podem ser iguais");
        
    }
}