using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs
{
    public class CreateCategoriaProcedimentoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class CreateCategoriaProcedimentoValidator : AbstractValidator<CreateCategoriaProcedimentoRequest>
    {
        public CreateCategoriaProcedimentoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(100);
        }
    }
}
