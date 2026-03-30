using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.TaxasIva.TaxaIvaService.Filters
{
    public class TaxaIvaAllFilter
    {
        private List<TableFilter>? _filters;
        private List<TanstackColumnOrder>? _sorting;

        public List<TableFilter> Filters
        {
            get => _filters ??= new List<TableFilter>();
            set => _filters = value ?? new List<TableFilter>();
        }

        public List<TanstackColumnOrder> Sorting
        {
            get => _sorting ??= new List<TanstackColumnOrder>();
            set => _sorting = value ?? new List<TanstackColumnOrder>();
        }

        public TaxaIvaAllFilter()
        {
            Filters = new List<TableFilter>();
            Sorting = new List<TanstackColumnOrder>();
        }

        public string GetOrderByString()
        {
            if (Sorting.Count == 0)
                return "";

            var validSortColumns = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
            if (validSortColumns.Count == 0)
                return "";

            string sortingString = "";
            int count = 1;
            foreach (TanstackColumnOrder sortColumn in validSortColumns)
            {
                sortingString += sortColumn.Desc ? "-" + sortColumn.Id : sortColumn.Id;
                if (count != validSortColumns.Count)
                    sortingString += ",";
                count++;
            }
            return sortingString;
        }
    }
}
