using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Tratamentos.FechoDiarioTratamentoAdministrativoService.DTOs;

public class FechoDiarioTratamentoRequest : IDto
{
    public DateTime? Data { get; set; }
}

public class FechoDiarioTratamentoRequestValidator : AbstractValidator<FechoDiarioTratamentoRequest>
{
    public FechoDiarioTratamentoRequestValidator()
    {
        _ = RuleFor(x => x.Data).NotNull();
    }
}