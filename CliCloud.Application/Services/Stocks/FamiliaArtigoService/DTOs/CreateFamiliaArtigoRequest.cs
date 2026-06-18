using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class CreateFamiliaArtigoRequest : IDto 
{
    public required string Descricao { get; set; }
    public Guid? ParentId { get; set; }
    public string? UrlFoto { get; set; }
}

public class CreateFamiliaArtigoValidator : AbstractValidator<CreateFamiliaArtigoRequest>
{
    public CreateFamiliaArtigoValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UrlFoto).MaximumLength(512);
    }
}