using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FeriadoService.DTOs;

public class ImportFeriadosRequest : IDto
{
    public Guid ClinicaOrigemId { get; set; }
}

public class ImportFeriadosRequestValidator : AbstractValidator<ImportFeriadosRequest>
{
    public ImportFeriadosRequestValidator()
    {
        RuleFor(x => x.ClinicaOrigemId)
            .NotEmpty()
            .WithMessage("Clínica de origem é obrigatória");
    }
}