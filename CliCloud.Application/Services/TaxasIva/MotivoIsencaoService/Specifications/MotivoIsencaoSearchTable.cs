using Ardalis.Specification;
using CliCloud.Application.Common.Filter;
using CliCloud.Application.Common.Specification;
using CliCloud.Domain.Entities.TaxasIva;

namespace CliCloud.Application.Services.TaxasIva.MotivoIsencaoService.Specifications
{
    public class MotivoIsencaoSearchTable : Specification<MotivoIsencao>
    {
        public MotivoIsencaoSearchTable(List<TableFilter> filters, string? dynamicOrder = "")
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var f in filters)
                {
                    switch (f.Id.ToLowerInvariant())
                    {
                        case "codigo":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Codigo.Contains(f.Value));
                            break;
                        case "codigosaft":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.CodigoSaft != null && x.CodigoSaft.Contains(f.Value));
                            break;
                        case "descricao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Descricao.Contains(f.Value));
                            break;
                        case "norma":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Norma != null && x.Norma.Contains(f.Value));
                            break;
                        case "mencao":
                            if (!string.IsNullOrWhiteSpace(f.Value))
                                _ = Query.Where(x => x.Mencao != null && x.Mencao.Contains(f.Value));
                            break;
                    }
                }
            }

            if (string.IsNullOrEmpty(dynamicOrder))
                _ = Query.OrderBy(x => x.Codigo);
            else
                _ = Query.OrderBy(dynamicOrder);
        }
    }
}
