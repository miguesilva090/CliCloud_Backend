using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs
{
    public class CreatePeriocidadeTratamentoRequest : IDto
    {
        public string? Descricao { get; set; }
    }

    public class CreatePeriocidadeTratamentoValidator : AbstractValidator<CreatePeriocidadeTratamentoRequest>
    {
        public CreatePeriocidadeTratamentoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}
