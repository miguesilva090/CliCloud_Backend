using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FeriadoService.DTOs;

public class InsertFeriadosAnoRequest : IDto 
{
    public int Ano { get; set; }
}

public class InsertFeriadosAnoRequestValidator : AbstractValidator<InsertFeriadosAnoRequest>
{
    public InsertFeriadosAnoRequestValidator()
    {
        RuleFor(x => x.Ano)
            .InclusiveBetween(1900, 3000)
            .WithMessage("Ano inválido");
    }
}