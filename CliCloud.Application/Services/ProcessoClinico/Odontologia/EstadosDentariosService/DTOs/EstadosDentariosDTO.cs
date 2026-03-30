using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.ProcessoClinico.Odontologia.EstadosDentariosService.DTOs
{
    public class EstadosDentariosDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descricao { get; set; }
        public bool EstadoPadrao { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

