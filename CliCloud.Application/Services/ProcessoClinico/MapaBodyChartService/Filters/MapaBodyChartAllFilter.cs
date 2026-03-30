using System.Linq;
using CliCloud.Application.Common.Filter;

namespace CliCloud.Application.Services.ProcessoClinico.MapaBodyChartService.Filters
{
    public class MapaBodyChartAllFilter
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

        public MapaBodyChartAllFilter()
        {
            Filters = new List<TableFilter>();
            Sorting = new List<TanstackColumnOrder>();
        }

        public string GetOrderByString()
        {
            if(Sorting.Count == 0) 
            {
                return "";
            }

            var validSortColumns = Sorting.Where(sc => !string.IsNullOrWhiteSpace(sc.Id)).ToList();
            if(validSortColumns.Count == 0)
            {
                return "";
            }

            string sortingString = "";
            int numberOfColumns = validSortColumns.Count;
            int count = 1;

            foreach(TanstackColumnOrder sortColumn in validSortColumns)
            {
                if(sortColumn.Desc)
                {
                    sortingString += "-" + sortColumn.Id;
                }
                else 
                {
                    sortingString += sortColumn.Id;
                }
                if(count != numberOfColumns)
                {
                    sortingString += ",";
                }
                count++;
            }
            return sortingString;
        }
    }
}