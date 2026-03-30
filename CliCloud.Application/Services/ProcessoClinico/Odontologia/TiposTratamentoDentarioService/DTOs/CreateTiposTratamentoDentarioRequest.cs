using FluentValidation;
using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs
{
    public class CreateTiposTratamentoDentarioRequest : IDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Faturavel { get; set; }
        public string? CodigoServicoAssociado { get; set; }
        public string? NomeServicoAssociado { get; set; }
    }

    public class CreateTiposTratamentoDentarioValidator : AbstractValidator<CreateTiposTratamentoDentarioRequest>
    {
        public CreateTiposTratamentoDentarioValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}
