using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.TiposTratamentoDentarioService.DTOs
{
    public class TiposTratamentoDentarioDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public bool Faturavel { get; set; }
        public string? CodigoServicoAssociado { get; set; }
        public string? NomeServicoAssociado { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

