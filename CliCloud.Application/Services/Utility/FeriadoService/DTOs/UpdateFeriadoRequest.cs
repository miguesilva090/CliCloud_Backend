using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FeriadoService.DTOs;

public class UpdateFeriadoRequest : IDto 
{
    public DateTime Data { get; set; }
    public string Designacao { get; set; } = string.Empty;
    public bool Ativo { get; set; }= true;
}

public class UpdateFeriadoRequestValidator : AbstractValidator<UpdateFeriadoRequest>
{
    public UpdateFeriadoRequestValidator()
    {
        RuleFor(x => x.Data).NotEmpty();
        RuleFor(x => x.Designacao).NotEmpty().MaximumLength(150);
    }
}