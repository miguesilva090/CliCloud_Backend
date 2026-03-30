using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.CategoriaProcedimentoService.DTOs
{
    public class UpdateCategoriaProcedimentoRequest : IDto
    {
        public required string Descricao { get; set; }
    }

    public class UpdateCategoriaProcedimentoValidator : AbstractValidator<UpdateCategoriaProcedimentoRequest>
    {
        public UpdateCategoriaProcedimentoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty().MaximumLength(100);
        }
    }
}
