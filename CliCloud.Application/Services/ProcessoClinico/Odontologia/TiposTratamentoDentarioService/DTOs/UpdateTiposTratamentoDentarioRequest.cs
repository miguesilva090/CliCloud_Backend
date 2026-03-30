using FluentValidation;
using CliCloud.Application.Common.Marker;


namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs
{
    public class UpdateTiposTratamentoDentarioRequest : IDto
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Faturavel { get; set; }
        public string? CodigoServicoAssociado { get; set; }
        public string? NomeServicoAssociado { get; set; }
        public bool Ativo { get; set; }
    }

    public class UpdateTiposTratamentoDentarioValidator : AbstractValidator<UpdateTiposTratamentoDentarioRequest>
    {
        public UpdateTiposTratamentoDentarioValidator()
        {
            _ = RuleFor(x => x.Codigo).NotEmpty();
            _ = RuleFor(x => x.Descricao).NotEmpty();
        }
    }
}

