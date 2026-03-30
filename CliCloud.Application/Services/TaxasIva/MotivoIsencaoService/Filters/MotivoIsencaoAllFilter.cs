using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Filters
{
    public class MotivoIsencaoAllFilter
    {
        private List<TableFilter>? _filters;
        private List<TanstackColumnOrder>? _sorting;

        public List<TableFilter> Filters { get => _filters ??= []; set => _filters = value ?? []; }
        public List<TanstackColumnOrder> Sorting { get => _sorting ??= []; set => _sorting = value ?? []; }

        public MotivoIsencaoAllFilter()
        {
            Filters = [];
            Sorting = [];
        }

        public string GetOrderByString()
        {
            if (Sorting.Count == 0) return "";
            var valid = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
            if (valid.Count == 0) return "";
            return string.Join(",", valid.Select(s => (s.Desc ? "-" : "") + s.Id));
        }
    }
}
