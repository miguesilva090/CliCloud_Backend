using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Documentos.NaturezaDocumentoService.Filters
{
    public class NaturezaDocumentoAllFilter
    {
        private List<TableFilter>? _filters;
        private List<TanstackColumnOrder>? _sorting;

        public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
        public List<TanstackColumnOrder> Sorting { get => _sorting ??= []; set => _sorting = value ?? []; }

        public NaturezaDocumentoAllFilter()
        {
            Filters = [];
            Sorting = [];
        }

        public string GetOrderByString()
        {
            if (Sorting.Count == 0) return "";
            List<TanstackColumnOrder> valid = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
            if (valid.Count == 0) return "";
            return string.Join(",", valid.Select(s => (s.Desc ? "-" : "") + s.Id));
        }
    }
}
