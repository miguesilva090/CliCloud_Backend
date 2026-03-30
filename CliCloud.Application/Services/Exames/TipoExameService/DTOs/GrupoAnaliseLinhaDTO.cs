using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class GrupoAnaliseLinhaDTO : IDto
    {
        public Guid Id { get; set; }
        public Guid AnaliseId { get; set; }
        public string? AnaliseNome { get; set; }
        public int Ordem { get; set; }
        public string? Descricao { get; set; }
        public string? UnidadeMedida { get; set; }
        public string? ValoresReferencia { get; set; }
    }
}
