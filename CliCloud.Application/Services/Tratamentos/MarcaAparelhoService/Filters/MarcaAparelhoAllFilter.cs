using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.Tratamentos.MarcaAparelhoService.Filters
{
    public class MarcaAparelhoAllFilter
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

        public MarcaAparelhoAllFilter()
        {
            Filters = new List<TableFilter>();
            Sorting = new List<TanstackColumnOrder>();
        }

        public string GetOrderByString()
        {
            if(Sorting.Count == 0) 
                return "";

            var validSortColumns = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
            if(validSortColumns.Count == 0) 
                return "";

            string sortingString = "";
            int count = 1;

            foreach(TanstackColumnOrder sortColumn in validSortColumns)
            {
                sortingString += sortColumn.Desc ? "-" + sortColumn.Id : sortColumn.Id;
                if(count != validSortColumns.Count)
                    sortingString += ",";
                count++;
            }
            return sortingString;
        }
    }
}