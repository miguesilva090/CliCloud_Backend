using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs
{
    public class UpdatePeriocidadeTratamentoRequest : IDto
    {
        public string? Descricao { get; set; }
    }

    public class UpdatePeriocidadeTratamentoValidator : AbstractValidator<UpdatePeriocidadeTratamentoRequest>
    {
        public UpdatePeriocidadeTratamentoValidator()
        {
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}

