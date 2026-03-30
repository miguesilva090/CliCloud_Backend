using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs
{
    public class CreateEstadosDentariosRequest : IDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EstadoPadrao { get; set; }
    }

    public class CreateEstadosDentariosValidator : AbstractValidator<CreateEstadosDentariosRequest>
    {
        public CreateEstadosDentariosValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}
