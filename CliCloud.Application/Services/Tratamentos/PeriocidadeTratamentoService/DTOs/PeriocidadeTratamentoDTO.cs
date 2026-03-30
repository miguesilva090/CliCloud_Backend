using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs
{
    public class PeriocidadeTratamentoDTO : IDto
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}

