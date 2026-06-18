using CliCloud.Application.Common.Marker;
using FluentValidation;

namespace CliCloud.Application.Services.Stocks.FamiliaArtigoService.DTOs;

public class UpdateFamiliaArtigoRequest : IDto 
{
    public required string Descricao { get; set; }
    public string? UrlFoto { get; set; }

}

public class UpdateFamiliaArtigoValidator : AbstractValidator<UpdateFamiliaArtigoRequest>
{
    public UpdateFamiliaArtigoValidator()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UrlFoto).MaximumLength(512);
    }
}