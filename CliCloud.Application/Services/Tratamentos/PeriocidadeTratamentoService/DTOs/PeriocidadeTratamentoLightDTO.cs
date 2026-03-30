using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Tratamentos.PeriocidadeTratamentoService.DTOs
{
    public class PeriocidadeTratamentoLightDTO : IDto 
    {
        public Guid Id { get; set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}