using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs
{
    public class UpdateEstadosDentariosRequest : IDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool EstadoPadrao { get; set; }
        public bool Ativo { get; set; }
    }

    public class UpdateEstadosDentariosValidator : AbstractValidator<UpdateEstadosDentariosRequest>
    {
        public UpdateEstadosDentariosValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}

