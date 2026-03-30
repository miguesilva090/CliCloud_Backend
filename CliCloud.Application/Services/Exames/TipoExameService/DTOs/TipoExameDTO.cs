using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class TipoExameDTO : IDto
    {
        public Guid Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public Guid? CategoriaProcedimentoId { get; set; }
        public Guid? TaxaIvaId { get; set; }
        public string? EAN { get; set; }
        public decimal Preco { get; set; }
        public Guid? MotivoIsencaoId { get; set; }
        public string? RecomendacoesVariaveis { get; set; }
        public int? Laboratorio { get; set; }
        public bool Inativo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public List<GrupoAnaliseLinhaDTO>? GrupoAnaliseLinhas { get; set; }
    }
}
