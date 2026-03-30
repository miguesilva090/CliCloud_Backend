using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Especialidades.CategoriaEspecialidadeService.Filters
{
    public class CategoriaEspecialidadeTableFilter : PaginationFilter
    {
        public List<TableFilter> Filters { get; set; }

        public CategoriaEspecialidadeTableFilter()
        {
            Filters = [];
        }
    }
}
