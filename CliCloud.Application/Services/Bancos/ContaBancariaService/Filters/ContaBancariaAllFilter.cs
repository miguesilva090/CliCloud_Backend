using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Bancos.ContaBancariaService.Filters
{
    public class ContaBancariaAllFilter
    {
        private List<TableFilter>? _filters;
        private List<TanstackColumnOrder>? _sorting;

        public List<TableFilter> Filters
        {
            get => _filters ??= [];
            set => _filters = value ?? [];
        }

        public List<TanstackColumnOrder> Sorting
        {
            get => _sorting ??= [];
            set => _sorting = value ?? [];
        }

        public string GetOrderByString()
        {
            if (Sorting.Count == 0)
                return string.Empty;

            List<TanstackColumnOrder> valid = Sorting
                .Where(sc => !string.IsNullOrWhiteSpace(sc.Id))
                .ToList();
            if (valid.Count == 0)
                return string.Empty;

            return string.Join(",", valid.Select(sc => sc.Desc ? $"-{sc.Id}" : sc.Id));
        }
    }
}