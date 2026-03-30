using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Doencas.DoencaService.Filters
{
    public class DoencaTableFilter : PaginationFilter
    {
        public string? Keyword { get; set; }
        /// <summary>Filtrar filhos de um nó (navegação hierárquica capítulo → bloco → categoria).</summary>
        public Guid? ParentId { get; set; }
    }
}
