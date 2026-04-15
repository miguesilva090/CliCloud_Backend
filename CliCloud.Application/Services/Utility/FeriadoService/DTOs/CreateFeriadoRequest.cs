using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Utility.FeriadoService.DTOs;

public class CreateFeriadoRequest : IDto 
{
    public DateTime Data { get; set; }
    public string Designacao { get; set; } = string.Empty;

}

public class CreateFeriadoRequestValidator : AbstractValidator<CreateFeriadoRequest>
{
    public CreateFeriadoRequestValidator()
    {
        RuleFor(x => x.Data).NotEmpty();
        RuleFor(x => x.Designacao).NotEmpty().MaximumLength(150);
    }
}