using CliCloud.Application.Common.Marker;

namespace CliCloud.Application.Services.Exames.TipoExameService.DTOs
{
    public class UpdateGrupoAnaliseLinhaRequest
    {
        public Guid? Id { get; set; }
        public Guid AnaliseId { get; set; }
        public int Ordem { get; set; }
        public string? Descricao { get; set; }
        public string? UnidadeMedida { get; set; }
        public string? ValoresReferencia { get; set; }
    }

    public class UpdateTipoExameRequest : IDto
    {
        public string Designacao { get; set; } = string.Empty;
        public Guid? CategoriaProcedimentoId { get; set; }
        public Guid? TaxaIvaId { get; set; }
        public string? EAN { get; set; }
        public decimal Preco { get; set; }
        public Guid? MotivoIsencaoId { get; set; }
        public string? RecomendacoesVariaveis { get; set; }
        public int? Laboratorio { get; set; }
        public bool Inativo { get; set; }
        public List<UpdateGrupoAnaliseLinhaRequest> GrupoAnaliseLinhas { get; set; } = [];
    }
}
